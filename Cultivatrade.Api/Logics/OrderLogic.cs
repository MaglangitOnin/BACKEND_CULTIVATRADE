using Cultivatrade.Api.DatabaseConnection;
using Cultivatrade.Api.DTO;
using Cultivatrade.Api.Models;
using Cultivatrade.Api.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.Linq;

namespace Cultivatrade.Api.Logics
{
    public class OrderLogic
    {
        private readonly CultivatradeContext _context;
        private readonly Base64Resizer _base64Resizer;
        private readonly FilePath _filePath;
        private readonly ProductLogic _productLogic;

        public OrderLogic(CultivatradeContext context, Base64Resizer base64Resizer, FilePath filePath, ProductLogic productLogic)
        {
            _context = context;
            _base64Resizer = base64Resizer;
            _filePath = filePath;
            _productLogic = productLogic;
        }

        public List<OrderDTO_GET> GetInventory(Guid sellerId, string? search = "")
        {
            var data = (from p in _context.Products
                        from o in _context.Orders.Where(x => x.ProductId == p.ProductId).DefaultIfEmpty()
                        let productImage = (from pf in _context.ProductFiles
                                            where p.ProductId == pf.ProductId
                                            orderby pf.DateTimeCreated descending
                                            select pf).FirstOrDefault()
                        where p.SellerId == sellerId
                        group new { p, o, productImage } by p.ProductId into grouped
                        select new OrderDTO_GET
                        {
                            SellerId = sellerId,
                            ProductId = grouped.Key,
                            ProductName = grouped.FirstOrDefault().p.Name,
                            ProductDescription = grouped.FirstOrDefault().p.Description,
                            ProductPrice = grouped.FirstOrDefault().p.Price,
                            Quantity = grouped.FirstOrDefault().p.Quantity,
                            TotalSold = grouped.Sum(g => g.o.QuantityBought), // Aggregate TotalSold
                            ProductImage = _base64Resizer.ResizeImage(_filePath.ProductImage(grouped.FirstOrDefault().productImage.Image)),
                            DeliveryDate = grouped.Where(g => g.o != null).OrderByDescending(g => g.o.DeliveryDate).Select(g => g.o.DeliveryDate).FirstOrDefault(),
                            PaymentOption = grouped.Where(g => g.o != null).OrderByDescending(g => g.o.DeliveryDate).Select(g => g.o.PaymentOption).FirstOrDefault(),
                            OrderStatus = grouped.Where(g => g.o != null).OrderByDescending(g => g.o.DeliveryDate).Select(g => g.o.OrderStatus).FirstOrDefault(),
                            DeliveryAddress = grouped.Where(g => g.o != null).OrderByDescending(g => g.o.DeliveryDate).Select(g => g.o.DeliveryAddress).FirstOrDefault(),
                            DeliveryCourier = grouped.Where(g => g.o != null).OrderByDescending(g => g.o.DeliveryDate).Select(g => g.o.DeliveryCourier).FirstOrDefault(),
                        });

            if (!string.IsNullOrWhiteSpace(search))
            {
                data = data.Where(x => x.ProductName.Contains(search));
            }

            return data.ToList();
        }



        public bool AddMultipleOrder(List<OrderDTO_POST> dto)
        {
            int success = 0;

            // ORDER
            var data = dto.Select(x =>
            {
                var orderId = Guid.NewGuid(); 
                var paymentId = Guid.NewGuid();

                var order = new Order
                {
                    OrderId = orderId,
                    BuyerId = x.BuyerId,
                    SellerId = x.SellerId,
                    ProductId = x.ProductId,
                    OrderDate = DateTime.Now,
                    OrderStatus = "Pending",
                    PaymentOption = x.PaymentOption,
                    QuantityBought = x.QuantityBought,
                    TotalAmount = x.TotalAmount,
                    DeliveryAddress = x.DeliveryAddress
                };

                var payment = new Payment
                {
                    PaymentId = paymentId,
                    OrderId = orderId, 
                    BuyerId = x.BuyerId,
                    PaymentSource = x.PaymentSource,
                    PaymentCodeId = x.PaymentCodeId,
                    PayerId = x.PayerId, 
                    DateTimeCreated = DateTime.Now,
                    TotalAmount = x.TotalAmount
                };

                var notification = new Notification
                {
                    NotificationId = Guid.NewGuid(),
                    UserId = x.SellerId,
                    BuyerId = x.BuyerId,
                    ProductId = x.ProductId,
                    OrderId = orderId, 
                    Message = "Product bought waiting for seller to prepare your order",
                    DateTimeCreated = DateTime.Now
                };
                _productLogic.UpdateProductQuantity(x.ProductId, x.QuantityBought, false);
                _context.Orders.Add(order);
                _context.Payments.Add(payment);
                _context.Notifications.Add(notification);

                return order; 
            }).ToList();

            success = _context.SaveChanges();

            return success > 0;
        }


        public Guid AddOrder(OrderDTO_POST dto)
        {
            int success = 0;
            var data = new Order();
            Guid orderId = Guid.NewGuid();

            data.OrderId = orderId;
            data.BuyerId = dto.BuyerId;
            data.SellerId = dto.SellerId;
            data.ProductId = dto.ProductId;
            data.OrderDate = DateTime.Now;
            data.OrderStatus = "Pending";
            data.PaymentOption = dto.PaymentOption;
            data.QuantityBought = dto.QuantityBought;
            data.TotalAmount = dto.TotalAmount;
            data.DeliveryAddress = dto.DeliveryAddress;

            _context.Orders.Add(data);

          
            var payment = new Payment();
            payment.PaymentId = Guid.NewGuid(); //
            payment.OrderId = orderId;
            payment.BuyerId = dto.BuyerId;
            payment.PaymentSource = dto.PaymentSource; //
            payment.PaymentCodeId = dto.PaymentCodeId;//
            payment.PayerId = dto.PayerId; //
            payment.DateTimeCreated = DateTime.Now; //
            payment.TotalAmount = dto.TotalAmount;

            _context.Payments.Add(payment);
            

            _context.SaveChanges();

            return orderId;
        }

        public List<OrderDTO_GET> GetOrder(Guid buyerId, Guid sellerId, string? search = "")
        {
            if (buyerId != Guid.Empty)
            {
                var data = (from o in _context.Orders
                            join p in _context.Products on o.ProductId equals p.ProductId
                            join b in _context.Users on o.BuyerId equals b.UserId
                            join s in _context.Users on o.SellerId equals s.UserId
                            let productImage = (from pf in _context.ProductFiles
                                                where p.ProductId == pf.ProductId
                                                orderby pf.DateTimeCreated descending
                                                select new { pf }).FirstOrDefault()
                            where o.BuyerId == buyerId
                            select new OrderDTO_GET
                            {
                                OrderId = o.OrderId,
                                BuyerId = o.BuyerId,
                                SellerId = o.SellerId,
                                ProductId = o.ProductId,
                                ProductName = p.Name,
                                ProductDescription = p.Description,
                                ProductPrice = p.Price,
                                QuantityBought = o.QuantityBought,
                                TotalAmount = o.TotalAmount,
                                DeliveryDate = o.DeliveryDate != null ? o.DeliveryDate : null,
                                BuyerFirstname = b.Firstname,
                                BuyerLastname = b.Lastname,
                                SellerFirstname = s.Firstname,
                                SellerLastname = s.Lastname,
                                SellerPhone = s.Phone,
                                SellerAddress = s.Address,
                                PaymentOption = o.PaymentOption,
                                OrderStatus = o.OrderStatus,
                                BuyerImage = _base64Resizer.ResizeImage(_filePath.UserImage(b.ProfileImage)),
                                SellerImage = _base64Resizer.ResizeImage(_filePath.UserImage(s.ProfileImage)),
                                ProductImage = _base64Resizer.ResizeImage(_filePath.ProductImage(productImage.pf.Image)),
                                DeliveryAddress = o.DeliveryAddress,
                                DeliveryCourier = o.DeliveryCourier
                            });
                if (!string.IsNullOrWhiteSpace(search))
                {
                    data = data.Where(x => x.ProductName.Contains(search) || x.SellerFirstname.Contains(search) || x.SellerLastname.Contains(search));
                    return data.ToList();
                }
                return data.ToList();
            }
            else if (sellerId != Guid.Empty)
            {
                var data = (from o in _context.Orders
                            join p in _context.Products on o.ProductId equals p.ProductId
                            join b in _context.Users on o.BuyerId equals b.UserId
                            join s in _context.Users on o.SellerId equals s.UserId
                            let productImage = (from pf in _context.ProductFiles
                                                where p.ProductId == pf.ProductId
                                                orderby pf.DateTimeCreated descending
                                                select new { pf }).FirstOrDefault()
                            where o.SellerId == sellerId
                            select new OrderDTO_GET
                            {
                                OrderId = o.OrderId,
                                BuyerId = o.BuyerId,
                                SellerId = o.SellerId,
                                ProductId = o.ProductId,
                                ProductName = p.Name,
                                ProductDescription = p.Description,
                                ProductPrice = p.Price,
                                QuantityBought = o.QuantityBought,
                                TotalAmount = o.TotalAmount,
                                DeliveryDate = o.DeliveryDate != null ? o.DeliveryDate : null,
                                BuyerFirstname = b.Firstname,
                                BuyerLastname = b.Lastname,
                                SellerFirstname = s.Firstname,
                                SellerLastname = s.Lastname,
                                SellerPhone = s.Phone,
                                SellerAddress = s.Address,
                                PaymentOption = o.PaymentOption,
                                OrderStatus = o.OrderStatus,
                                BuyerImage = _base64Resizer.ResizeImage(_filePath.UserImage(b.ProfileImage)),
                                SellerImage = _base64Resizer.ResizeImage(_filePath.UserImage(s.ProfileImage)),
                                ProductImage = _base64Resizer.ResizeImage(_filePath.ProductImage(productImage.pf.Image)),
                                DeliveryAddress = o.DeliveryAddress,
                                DeliveryCourier = o.DeliveryCourier
                            });
                if (!string.IsNullOrWhiteSpace(search))
                {
                    data = data.Where(x => x.ProductName.Contains(search) || x.SellerFirstname.Contains(search) || x.SellerLastname.Contains(search));
                    return data.ToList();
                }
                return data.ToList();
            }
            return new List<OrderDTO_GET>();
        }

        public bool UpdateOrderStatus(Guid orderId, OrderDTO_PUT dto)
        {
            int success = 0;
            var data = _context.Orders.FirstOrDefault(x => x.OrderId == orderId);

            data.OrderStatus = dto.OrderStatus;
            data.OrderDateReceived = dto.OrderDateReceived;
            data.DeliveryDate = dto.DeliveryDate;
            data.DeliveryCourier = dto.DeliveryCourier;

            _context.Orders.Update(data);
            success = _context.SaveChanges();

            return success > 0;
        }


    }
}
