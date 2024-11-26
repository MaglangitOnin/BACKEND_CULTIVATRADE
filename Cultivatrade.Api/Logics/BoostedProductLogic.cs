using Cultivatrade.Api.DatabaseConnection;
using Cultivatrade.Api.DTO;
using Cultivatrade.Api.Models;
using Cultivatrade.Api.Services;

namespace Cultivatrade.Api.Logics
{
    public class BoostedProductLogic
    {
        private readonly CultivatradeContext _context;

        public BoostedProductLogic(CultivatradeContext context)
        {
            _context = context;
        }

        public List<BoostedProductDTO_GET> GetBoostProductByProductId(Guid productId)
        {
            DateTime currentDate = DateTime.Now;
            var data = (from b in _context.BoostedProducts
                        where b.ProductId == productId
                        select new BoostedProductDTO_GET
                        {
                            NumberOfDays = b.NumberOfDays,
                            BoostCost = b.BoostCost,
                            DateTimeCreated = b.DateTimeCreated,
                            ExpirationDate = b.DateTimeCreated.AddDays(b.NumberOfDays),
                            IsExpired = b.DateTimeCreated.AddDays(b.NumberOfDays) < currentDate
                        }).ToList();
            return data;
        }

        public bool AddBoostProduct(BoostedProductDTO_POST dto)
        {
            int success = 0;

            var data = new BoostedProduct
            {
                BoostedProductId = Guid.NewGuid(),
                ProductId = dto.ProductId,
                BoostCost = dto.BoostCost,
                NumberOfDays = dto.NumberOfDays,
                DateTimeCreated = DateTime.Now
            };

            _context.BoostedProducts.Add(data);
            success = _context.SaveChanges();

            return success > 0;
        }
    }
}
