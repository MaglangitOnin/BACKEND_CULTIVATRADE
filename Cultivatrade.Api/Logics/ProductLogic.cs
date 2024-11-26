using Cultivatrade.Api.DatabaseConnection;
using Cultivatrade.Api.DTO;
using Cultivatrade.Api.Models;
using Cultivatrade.Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Cultivatrade.Api.Logics
{
    public class ProductLogic
    {
        private readonly CultivatradeContext _context;
        private readonly Base64Resizer _base64Resizer;
        private readonly FilePath _filePath;
        public ProductLogic(CultivatradeContext context, Base64Resizer base64Resizer, FilePath filePath)
        {
            _context = context;
            _base64Resizer = base64Resizer;
            _filePath = filePath;
        }

       

        // GET PRODUCT
        public List<ProductDTO_GET> GetProduct(Guid userId, string? search = "")
        {
            var currentDate = DateTime.Now;

            var data = (from p in _context.Products
                        join u in _context.Users on p.SellerId equals u.UserId
                        where p.IsDeleted == false && p.SellerId != userId
                        let productImage = (from pf in _context.ProductFiles
                                            where p.ProductId == pf.ProductId
                                            orderby pf.DateTimeCreated descending
                                            select new { pf }).FirstOrDefault()
                        let boostedProducts = (from b in _context.BoostedProducts
                                               where b.ProductId == p.ProductId
                                               select new BoostedProductDTO_GET
                                               {
                                                   NumberOfDays = b.NumberOfDays,
                                                   DateTimeCreated = b.DateTimeCreated,
                                                   ExpirationDate = b.DateTimeCreated.AddDays(b.NumberOfDays),
                                                   IsExpired = b.DateTimeCreated.AddDays(b.NumberOfDays) < currentDate
                                               }).ToList()
                        let reports = _context.Reports.Where(x=>x.ProductId == p.ProductId).Count()
                        where p.IsDeleted == false && p.IsDisabled == false &&  reports <= 5
                        orderby
                            boostedProducts.Any(b => !b.IsExpired) descending,
                            p.DateTimeCreated descending
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
                            NumberOfReport = _context.Reports.Where(x => x.ProductId == p.ProductId).Count(),

                            BoostExpiration = boostedProducts, 
                            IsExpired = boostedProducts.All(b => b.IsExpired),
                            IsDisabled = p.IsDisabled
                        }).ToList();

            if (!string.IsNullOrWhiteSpace(search))
            {
                data = data.Where(x => x.ProductName.Contains(search) || (x.SellerFirstname + " " + x.SellerLastname).Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
                // (x.SellerFirstname + " " + x.SellerLastname).Contains(search, StringComparison.OrdinalIgnoreCase)
            }

            return data;
        }


        // GET PRODUCT BY USER ID
        public List<ProductDTO_GET> GetProductByUserId(Guid userId, string? search = "")
        {
            var data = (from p in _context.Products
                        join u in _context.Users on p.SellerId equals u.UserId
                        where p.SellerId == userId && p.IsDeleted == false
                        
                        let productImage = (from pf in _context.ProductFiles
                                            where p.ProductId == pf.ProductId
                                            orderby pf.DateTimeCreated descending
                                            select new { pf }).FirstOrDefault()
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
                            IsDisabled = p.IsDisabled,
                            Orders = (from o in _context.Orders
                                      where o.ProductId == p.ProductId
                                      select new Order
                                      {
                                          OrderStatus = o.OrderStatus
                                      }).ToList(),
                            FarmName = u.FarmName,
                            FarmAddress = u.FarmAddress,
                            FarmDescription = u.FarmDescription,
                            BusinessPermitImage = _base64Resizer.ResizeImage(_filePath.BusinessPermitImage(u.BusinessPermitImage)),

                        }).ToList();
            if (!string.IsNullOrWhiteSpace(search))
            {
                data = data.Where(x => x.ProductName.Contains(search)).ToList();
                return data;
            }


            return data;
        }

        // GET PRODUCT BY PRODUCT ID
        public ProductDTO_GET GetProductByProductId(Guid productId)
        {
            var data = (from p in _context.Products
                        join u in _context.Users on p.SellerId equals u.UserId
                        where p.ProductId == productId && p.IsDeleted == false
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
                                SellerImage = _base64Resizer.ResizeImage(_filePath.UserImage(u.ProfileImage)),
                                ProductFiles = (from pf in _context.ProductFiles
                                                where pf.ProductId == productId
                                                select new ProductFileDTO_GET
                                                {
                                                    ProductFileId = pf.ProductFileId
                                                }).ToList(),
                               
                            }).FirstOrDefault();

                    if (data != null)
                    {
                        data.SellerImage = $"data:image/png;base64,{data.SellerImage}";
                    }

            return data;
        }

        // ADD PRODUCT
        public bool AddProduct(ProductDTO_POST dto)
        {
            int success = 0;

            var data = new Product();
            data.ProductId = dto.ProductId;
            data.SellerId = dto.SellerId;
            data.CategoryName = dto.CategoryName;
            data.Name = dto.ProductName;
            data.Description = dto.ProductDescription;
            data.Price = dto.ProductPrice;
            data.Quantity = dto.Quantity;
            data.ExpiryDate = dto.ExpiryDate;
            data.DateTimeCreated = DateTime.Now;
            data.IsDeleted = false;

            _context.Products.Add(data);
            success = _context.SaveChanges();

            if(success > 0)
            {
                return true;
            }
            return false;
        }

        // UPDATE PRODUCT
        public bool UpdateProduct(Guid productId, ProductDTO_PUT dto)
        {
            int success = 0;

            var data = _context.Products.FirstOrDefault(x => x.ProductId == productId);
            data.CategoryName = dto.CategoryName;
            data.Name = dto.ProductName;
            data.Description = dto.ProductDescription;
            data.Price = dto.ProductPrice;
            data.Quantity = dto.Quantity;
            data.ExpiryDate = dto.ExpiryDate;
            
            _context.Products.Update(data);
            success = _context.SaveChanges();

            if (success > 0)
            {
                return true;
            }
            return false;
        }

        // DELETE PRODUCT
        public bool DeleteProduct(Guid productId)
        {
            int success = 0;
            var data = _context.Products.FirstOrDefault(x => x.ProductId == productId);
            data.IsDeleted = true;

            _context.Products.Update(data);
            success = _context.SaveChanges();

            if (success > 0)
            {
                return true;
            }
            return false;
        }

        // DISABLE PRODUCT
        public bool DisableProduct(Guid productId)
        {
            int success = 0;
            var data = _context.Products.FirstOrDefault(x => x.ProductId == productId);
            data.IsDisabled = !data.IsDisabled;

            _context.Products.Update(data);
            success = _context.SaveChanges();

            if (success > 0)
            {
                return true;
            }
            return false;
        }

        // GET PROCUCT
        public int GetProduct()
        {
            var data = _context.Products.Where(x => x.IsDeleted == false).ToList().Count();
            return data;
        }

        public bool UpdateProductQuantity(Guid productId, int quantity, bool isInventory)
        {
            int success = 0;
            var data = _context.Products.FirstOrDefault(x => x.ProductId == productId);

            if (isInventory)
            {
                data.Quantity += quantity;
            }
            if (!isInventory)
            {
                data.Quantity -= quantity;
            }
            

            _context.Products.Update(data);
            success = _context.SaveChanges();

            if (success > 0)
            {
                return true;
            }
            return false;
        }
    }
}
