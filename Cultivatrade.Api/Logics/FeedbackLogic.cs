using Cultivatrade.Api.DatabaseConnection;
using Cultivatrade.Api.DTO;
using Cultivatrade.Api.Models;
using Cultivatrade.Api.Services;

namespace Cultivatrade.Api.Logics
{
    public class FeedbackLogic
    {
        private readonly CultivatradeContext _context;
        private readonly Base64Resizer _base64Resizer;
        private readonly FilePath _filePath;
        public FeedbackLogic(CultivatradeContext context, Base64Resizer base64Resizer, FilePath filePath)
        {
            _context = context;
            _base64Resizer = base64Resizer;
            _filePath = filePath;
        }

        //public List<FeedbackDTO_GET> GetFeedbackByProductId(Guid productId)
        //{
        //    var data = (from f in _context.Feedbacks
        //                join u in _context.Users on f.BuyerId equals u.UserId
        //                where f.ProductId == productId
        //                select new FeedbackDTO_GET
        //                {
        //                    Rating = f.Rating,
        //                    Message = f.Message,
        //                    BuyerName = $"{u.Firstname} {u.Lastname}",
        //                    BuyerImage = _base64Resizer.ResizeImage(_filePath.UserImage(u.ProfileImage))
        //                }).ToList();
        //    return data;
        //}

        public bool IsAlreadyRated(Guid userId, Guid productId)
        {
            var data = _context.Feedbacks.Any(x => x.BuyerId == userId && x.ProductId == productId);
            if (data)
            {
                return true;
            }
            return false;
        }

        public bool AddFeedback(FeedbackDTO_POST dto)
        {
            int success = 0;
            var data = new Feedback();
            data.FeedbackId = Guid.NewGuid();
            data.BuyerId = dto.BuyerId;
            data.ProductId = dto.ProductId;
            data.Message = dto.Message;
            data.Rating = dto.Rating;
            data.DateTimeCreated = DateTime.Now;

            _context.Feedbacks.Add(data);
            success = _context.SaveChanges();
            
            if(success > 0)
            {
                return true;
            }
            return false;
        }

    }
}
