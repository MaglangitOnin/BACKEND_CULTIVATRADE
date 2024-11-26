using Cultivatrade.Api.DatabaseConnection;
using Cultivatrade.Api.DTO;
using Cultivatrade.Api.Models;
using Cultivatrade.Api.Services;
using BC = BCrypt.Net.BCrypt;

namespace Cultivatrade.Api.Logics
{
    public class AdminLogic
    {
        private readonly CultivatradeContext _context;
        private readonly Base64Resizer _base64Resizer;
        private readonly FilePath _filePath;
        public AdminLogic(CultivatradeContext context, Base64Resizer base64Resizer, FilePath filePath)
        {
            _context = context;
            _base64Resizer = base64Resizer;
            _filePath = filePath;
        }

        // LOGIN ADMIN
        public Admin LoginAdmin(AdminDTO_LOGIN dto)
        {
            var data = _context.Admins.FirstOrDefault(x => x.Username == dto.Username);
            if (data != null)
            {
                if (BC.Verify(dto.Password, data.Password))
                {
                    return data;
                }
                return null;
            }
            return null;
        }

        public bool UpdatePassword(Guid adminId, AdminDTO_POST dto)
        {
            var data = _context.Admins.FirstOrDefault(x => x.AdminId == adminId);
            if(!BC.Verify(dto.CurrentPassword, data.Password))
            {
                throw new Exception("Invalid current password");
            }

            data.Password = BC.HashPassword(dto.NewPassword, BC.GenerateSalt());
            _context.Admins.Update(data);
            return _context.SaveChanges() > 0;

        }

        public List<ProductDTO_GET> GetProductByDateTimeCreated(string? search = "", int? year = null, int? month = null)
        {
            int targetYear = year ?? DateTime.Now.Year;
            int targetMonth = month ?? DateTime.Now.Month;

            var data = (from p in _context.Products
                        join u in _context.Users on p.SellerId equals u.UserId
                        let productImage = (from pf in _context.ProductFiles
                                            where p.ProductId == pf.ProductId
                                            orderby pf.DateTimeCreated descending
                                            select new { pf }).FirstOrDefault()
                        where p.DateTimeCreated.Year == targetYear && p.DateTimeCreated.Month == targetMonth && p.IsDeleted == false
                        orderby p.DateTimeCreated descending
                        select new ProductDTO_GET
                        {
                            ProductId = p.ProductId,
                            SellerId = p.SellerId,
                            SellerFirstname = u.Firstname,
                            SellerLastname = u.Lastname,
                            SellerEmail = u.Email,
                            SellerPhone = u.Phone,
                            SellerAddress = u.Address,
                            CategoryName = p.CategoryName,
                            ProductName = p.Name,
                            ProductDescription = p.Description,
                            ProductPrice = p.Price,
                            Quantity = p.Quantity,
                            ExpiryDate = p.ExpiryDate,
                            ProductImage = _base64Resizer.ResizeImage(_filePath.ProductImage(productImage.pf.Image)),
                            SellerImage = _base64Resizer.ResizeImage(_filePath.UserImage(u.ProfileImage)),
                            ProductFiles = (from pf in _context.ProductFiles
                                            where p.ProductId == pf.ProductId
                                            select new ProductFileDTO_GET
                                            {
                                                ProductFileId = pf.ProductFileId
                                            }).ToList(),
                            Feedbacks = (from f in _context.Feedbacks
                                         join u in _context.Users on f.BuyerId equals u.UserId
                                         where p.ProductId == f.ProductId
                                         select new FeedbackDTO_GET
                                         {
                                             Rating = f.Rating,
                                             Message = f.Message,
                                             BuyerName = $"{u.Firstname} {u.Lastname}",
                                             BuyerImage = _base64Resizer.ResizeImage(_filePath.UserImage(u.ProfileImage)),
                                             DateTimeCreated = f.DateTimeCreated
                                         }).ToList(),
                            IsDeleted = p.IsDeleted,
                            TotalSold = _context.Orders.Where(x=>x.ProductId == p.ProductId).Sum(x=>x.QuantityBought)
                        }).ToList();

            if (!string.IsNullOrWhiteSpace(search))
            {
                data = data.Where(x => x.ProductName.Contains(search)).ToList();
            }

            return data;
        }

        public object GetStatistics()
        {
            var totalUsers = _context.Users.ToList().Count();
            var totalProducts = _context.Products.ToList().Count();
            var revenue = _context.Orders.Sum(x => x.TotalAmount);
            var sold = _context.Orders.Where(x => x.OrderStatus == "Settle" || x.OrderStatus == "Received").ToList().Count();

            return new {
                Users = totalUsers,
                Products = totalProducts,
                Revenue = revenue,
                Sold = sold,

            };
        }

        public List<OrderDTO_GET> GetSoldProduct(string? search = "", int? year = null, int? month = null) 
        {
            int targetYear = year ?? DateTime.Now.Year;
            int targetMonth = month ?? DateTime.Now.Month;

            var data = (from o in _context.Orders
                        join p in _context.Products on o.ProductId equals p.ProductId
                        join b in _context.Users on o.BuyerId equals b.UserId
                        join s in _context.Users on o.SellerId equals s.UserId
                        let productImage = (from pf in _context.ProductFiles
                                            where p.ProductId == pf.ProductId
                                            orderby pf.DateTimeCreated descending
                                            select new { pf }).FirstOrDefault()
                        where o.OrderDateReceived.HasValue &&
                              o.OrderDateReceived.Value.Year == targetYear &&
                              o.OrderDateReceived.Value.Month == targetMonth &&
                              (o.OrderStatus == "Settle" || o.OrderStatus == "Received")

                        select new OrderDTO_GET
                        {
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

    }
}
