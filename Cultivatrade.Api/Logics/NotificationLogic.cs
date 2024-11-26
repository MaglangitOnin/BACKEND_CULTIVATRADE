using Cultivatrade.Api.DatabaseConnection;
using Cultivatrade.Api.DTO;
using Cultivatrade.Api.Models;
using Cultivatrade.Api.Services;
using Microsoft.AspNetCore.Hosting;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;

namespace Cultivatrade.Api.Logics
{
    public class NotificationLogic
    {
        private readonly CultivatradeContext _context;
        private readonly Base64Resizer _base64Resizer;
        private readonly FilePath _filePath;
        public NotificationLogic(CultivatradeContext context, Base64Resizer base64Resizer, FilePath filePath)
        {
            _context = context;
            _base64Resizer = base64Resizer;
            _filePath = filePath;
        }

        public bool AddNotification(NotificationDTO_POST dto)
        {
            int success = 0;

            var data = new Notification();

            data.NotificationId = Guid.NewGuid();
            data.UserId = dto.UserId;
            data.BuyerId = dto.BuyerId;
            data.ProductId = dto.ProductId;
            data.OrderId = dto.OrderId;
            data.Message = dto.Message;
            data.DateTimeCreated = DateTime.Now;

            _context.Notifications.Add(data);
            success = _context.SaveChanges();

            if(success > 0)
            {
                return true;
            }
            return false;
            
        }

        public List<NotificationDTO_GET> GetNotificationByBuyerId(Guid buyerId)
        {
            var data = (from n in _context.Notifications
                        from p in _context.Products.Where(x => x.ProductId == n.ProductId).DefaultIfEmpty()
                        from o in _context.Orders.Where(x => x.OrderId == n.OrderId).DefaultIfEmpty()
                        from b in _context.Users.Where(x => x.UserId == n.BuyerId).DefaultIfEmpty()
                        from s in _context.Users.Where(x => x.UserId == n.UserId).DefaultIfEmpty()
                        orderby n.DateTimeCreated descending
                        where n.BuyerId == buyerId
                        select new NotificationDTO_GET
                        {
                            SellerId = n.UserId,
                            BuyerId = n.BuyerId != null || n.BuyerId != Guid.Empty ? n.BuyerId : null,
                            NotificationId = n.NotificationId,
                            ProductId = n.ProductId,
                            ProductName = p.Name,
                            Message = n.Message,
                            DateTimeCreated = n.DateTimeCreated,
                            QuantityBought = o.QuantityBought,
                            TotalAmount = o.TotalAmount,
                            BuyerFirstname = b.Firstname,
                            BuyerLastname = b.Lastname,
                            SellerFirstname = s.Firstname,
                            SellerLastname = s.Lastname,
                            OrderStatus = o.OrderStatus,
                            BuyerImage = _base64Resizer.ResizeImage(_filePath.UserImage(b.ProfileImage)),
                            SellerImage = _base64Resizer.ResizeImage(_filePath.UserImage(s.ProfileImage)),

                        }).ToList();
            return data;
        }

        public List<NotificationDTO_GET> GetNotificationByUserId(Guid userId)
        {
            var data = (from n in _context.Notifications
                        from p in _context.Products.Where(x => x.ProductId == n.ProductId).DefaultIfEmpty()
                        from o in _context.Orders.Where(x => x.OrderId == n.OrderId).DefaultIfEmpty()
                        from b in _context.Users.Where(x => x.UserId == n.BuyerId).DefaultIfEmpty()
                        from s in _context.Users.Where(x => x.UserId == n.UserId).DefaultIfEmpty()
                        orderby n.DateTimeCreated descending
                        where n.UserId == userId
                        select new NotificationDTO_GET
                        {
                            SellerId = n.UserId,
                            BuyerId = n.BuyerId != null || n.BuyerId != Guid.Empty ? n.BuyerId : null,
                            NotificationId = n.NotificationId,
                            ProductId = n.ProductId,
                            ProductName = p.Name,
                            Message = n.Message,
                            DateTimeCreated = n.DateTimeCreated,
                            QuantityBought = o.QuantityBought,
                            TotalAmount = o.TotalAmount,
                            BuyerFirstname = b.Firstname,
                            BuyerLastname = b.Lastname,
                            SellerFirstname = s.Firstname,
                            SellerLastname = s.Lastname,
                            OrderStatus = o.OrderStatus,
                            BuyerImage = _base64Resizer.ResizeImage(_filePath.UserImage(b.ProfileImage)),
                            SellerImage = _base64Resizer.ResizeImage(_filePath.UserImage(s.ProfileImage)),


                        }).ToList();
            return data;
        }

        public bool DeleteNotification(Guid notificationId)
        {
            int success = 0;
            var data = _context.Notifications.FirstOrDefault(x => x.NotificationId == notificationId);

            _context.Notifications.Remove(data);
            success = _context.SaveChanges();

            if (success > 0)
            {
                return true;
            }
            return false;

        }

        //private static string GetImageAsBase64(string imagePath)
        //{
        //    if (!File.Exists(imagePath)) return null;

        //    using (var image = Image.Load(imagePath))
        //    {
        //        image.Mutate(x => x.Resize(50, 50));
        //        using (var ms = new MemoryStream())
        //        {
        //            image.SaveAsPng(ms);
        //            var base64String = Convert.ToBase64String(ms.ToArray());
        //            return $"data:image/png;base64,{base64String}";
        //        }
        //    }
        //}
    }
}
