using Cultivatrade.Api.DatabaseConnection;
using Cultivatrade.Api.DTO;
using Cultivatrade.Api.Models;
using System.Runtime.CompilerServices;

namespace Cultivatrade.Api.Logics
{
    public class ReportLogic
    {
        private readonly CultivatradeContext _context;

        public ReportLogic(CultivatradeContext context)
        {
            _context = context;
        }

        public bool IsAlreadyReported(Guid buyerId, Guid productId)
        {
            var data = _context.Reports.FirstOrDefault(x => x.BuyerId == buyerId && x.ProductId == productId);
            if(data != null)
            {
                return true;
            }
            return false;
        }
    

        public List<ReportDTO_GET> GetReportByProductId(Guid productId)
        {
            var data = (from r in _context.Reports
                        join u in _context.Users on r.BuyerId equals u.UserId
                        where r.ProductId == productId
                        select new ReportDTO_GET
                        {
                            Reason = r.Reason,
                            DateTimeCreated = r.DateTimeCreated,
                            BuyerFirstname = u.Firstname,
                            BuyerLastname = u.Lastname
                        }).ToList();
            return data;
        }

        public bool AddReport(ReportDTO_POST dto)
        {
            int success = 0;

            var data = new Report
            {
                ReportId = Guid.NewGuid(),
                ProductId = dto.ProductId,
                BuyerId = dto.BuyerId,
                Reason = dto.Reason,
                DateTimeCreated = DateTime.Now
            };

            _context.Reports.Add(data);
            success = _context.SaveChanges();

            return success > 0;
        }
    }
}
