using Cultivatrade.Api.DatabaseConnection;
using Cultivatrade.Api.DTO;
using Cultivatrade.Api.Models;
using Cultivatrade.Api.Services;
using Microsoft.AspNetCore.Hosting;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Cultivatrade.Api.Logics
{
    public class MessageLogic
    {
        private readonly CultivatradeContext _context;
        private readonly Base64Resizer _base64Resizer;
        private readonly FilePath _filePath;

        public MessageLogic(CultivatradeContext context, Base64Resizer base64Resizer, FilePath filePath)
        {
            _context = context;
            _base64Resizer = base64Resizer;
            _filePath = filePath;
        }

        public bool AddMessage(MessageDTO_POST dto)
        {
            int success = 0;
            var data = new Message();
            data.MessageId = Guid.NewGuid();
            data.BuyerId = dto.BuyerId;
            data.SellerId = dto.SellerId;
            data.Message1 = dto.Message;
            data.DateTimeCreated = DateTime.Now;

            _context.Messages.Add(data);
            success = _context.SaveChanges();
            return success > 0;
        }

        public List<MessageDTO_GET> GetMessages(Guid buyerId, Guid sellerId)
        {
            var data = (from m in _context.Messages
                        join buyer in _context.Users on m.BuyerId equals buyer.UserId
                        join seller in _context.Users on m.SellerId equals seller.UserId
                        
                        where (m.BuyerId == buyerId && m.SellerId == sellerId) ||
                              (m.SellerId == buyerId && m.BuyerId == sellerId)
                        orderby m.DateTimeCreated ascending
                        select new MessageDTO_GET
                        {
                            MessageId = m.MessageId,
                            Message = m.Message1,
                            SellerId = m.SellerId,
                            BuyerId = m.BuyerId,
                            DateTimeCreated = m.DateTimeCreated,
                            BuyerFirstname = buyer.Firstname,
                            BuyerLastname = buyer.Lastname,
                            SellerFirstname = seller.Firstname,
                            SellerLastname = seller.Lastname,
                            BuyerImage = _base64Resizer.ResizeImage(_filePath.UserImage(buyer.ProfileImage)),
                            SellerImage = _base64Resizer.ResizeImage(_filePath.UserImage(seller.ProfileImage)),
                        }).ToList();

            return data;
        }

        public List<MessageDTO_GET> GetContact(Guid buyerId, Guid sellerId)
        {
            // BUYER
            if (buyerId != Guid.Empty)
            {
                var query = (from m in _context.Messages
                             join u in _context.Users on m.SellerId equals u.UserId
                             where m.BuyerId == buyerId
                             orderby m.DateTimeCreated ascending
                             select new MessageDTO_GET
                             {
                                 MessageId = m.MessageId,
                                 Message = m.Message1,
                                 SellerId = m.SellerId,
                                 BuyerId = m.BuyerId,
                                 DateTimeCreated = m.DateTimeCreated,
                                 SellerImage = _base64Resizer.ResizeImage(_filePath.UserImage(u.ProfileImage)),
                                 SellerFirstname = u.Firstname,
                                 SellerLastname = u.Lastname
                             }).ToList();

                var data = query.GroupBy(x => x.SellerId).Select(g => g.First()).ToList();
                return data;
            }
            // SELLER
            else if (sellerId != Guid.Empty)
            {
                var query = (from m in _context.Messages
                             join u in _context.Users on m.BuyerId equals u.UserId
                             where m.SellerId == sellerId
                             orderby m.DateTimeCreated ascending
                             select new MessageDTO_GET
                             {
                                 MessageId = m.MessageId,
                                 Message = m.Message1,
                                 SellerId = m.SellerId,
                                 BuyerId = m.BuyerId,
                                 DateTimeCreated = m.DateTimeCreated,
                                 BuyerImage = _base64Resizer.ResizeImage(_filePath.UserImage(u.ProfileImage)),
                                 BuyerFirstname = u.Firstname,
                                 BuyerLastname = u.Lastname
                             }).ToList();

                var data = query.GroupBy(x => x.BuyerId).Select(g => g.First()).ToList();
                return data;
            }

            return new List<MessageDTO_GET>();
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
