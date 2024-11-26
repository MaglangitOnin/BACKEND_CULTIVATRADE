using Cultivatrade.Api.DatabaseConnection;
using Cultivatrade.Api.DTO;
using Cultivatrade.Api.Services;

namespace Cultivatrade.Api.Logics
{
    public class PaymentLogic
    {
        private readonly CultivatradeContext _context;
        private readonly Base64Resizer _base64Resizer;
        private readonly FilePath _filePath;
        public PaymentLogic(CultivatradeContext context, Base64Resizer base64Resizer, FilePath filePath)
        {
            _context = context;
            _base64Resizer = base64Resizer;
            _filePath = filePath;
        }

        public List<PaymentDTO_GET> GetPaymentByBuyerId(Guid buyerId)
        {
            var data = (from p in _context.Payments
                        join u in _context.Users on p.BuyerId equals u.UserId
                        join o in _context.Orders on p.OrderId equals o.OrderId
                        join prod in _context.Products on o.ProductId equals prod.ProductId
                        let productImage = (from pf in _context.ProductFiles
                                            where prod.ProductId == pf.ProductId
                                            orderby pf.DateTimeCreated descending
                                            select new { pf }).FirstOrDefault()
                        where u.UserId == buyerId
                        orderby p.DateTimeCreated descending
                        select new PaymentDTO_GET
                        {
                            PaymentId = p.PaymentId,
                            BuyerId = p.BuyerId,
                            OrderId = p.OrderId,
                            ProductName = prod.Name,
                            PaymentSource = p.PaymentSource,
                            PaymentCode = p.PaymentCodeId,
                            TotalAmount = p.TotalAmount,
                            DateTimeCreated = p.DateTimeCreated,
                            ProductFile = _base64Resizer.ResizeImage(_filePath.ProductImage(productImage.pf.Image))

                        }).ToList();
            return data;
        }
    }
}
