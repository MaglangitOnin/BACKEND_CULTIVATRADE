using Cultivatrade.Api.DatabaseConnection;
using Cultivatrade.Api.DTO;
using Cultivatrade.Api.Models;
using Cultivatrade.Api.Services;
using Microsoft.AspNetCore.Hosting;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Cultivatrade.Api.Logics
{
    public class CartLogic
    {
        private readonly CultivatradeContext _context;
        private readonly Base64Resizer _base64Resizer;
        private readonly FilePath _filePath;
        public CartLogic(CultivatradeContext context, Base64Resizer base64Resizer, FilePath filePath)
        {
            _context = context;
            _base64Resizer = base64Resizer;
            _filePath = filePath;
        }

        public void UpdateQuanityBought(Guid cartId, int quantityBought)
        {
            var data = _context.Carts.FirstOrDefault(x => x.CartId == cartId);
            data.Quantity = quantityBought;

            _context.Carts.Update(data);
            _context.SaveChanges();
        }

        // GET CART BY USER ID
        public List<CartDTO_GET> GetCartByUserId(Guid userId, string? search = "")
        {
            var data = (from c in _context.Carts
                        join u in _context.Users on c.BuyerId equals u.UserId
                        join p in _context.Products on c.ProductId equals p.ProductId
                        join s in _context.Users on p.SellerId equals s.UserId
                        orderby c.DateTimeCreated descending
                        where u.UserId == userId && p.IsDeleted == false && p.Quantity > 0
                        let productImage = (from pf in _context.ProductFiles
                                            where p.ProductId == pf.ProductId
                                            orderby pf.DateTimeCreated descending
                                            select new { pf }).FirstOrDefault()
                        select new CartDTO_GET
                        {
                            CartId = c.CartId,
                            SellerId = p.SellerId,
                            BuyerId = u.UserId,
                            ProductId = p.ProductId,
                            Quantity = c.Quantity,
                            ProductName = p.Name,
                            ProductDescription = p.Description,
                            ProductQuantity = p.Quantity,
                            ProductPrice = p.Price,
                            SellerFirstname = s.Firstname,
                            SellerLastname = s.Lastname,
                            SellerPhone = s.Phone,
                            SellerAddress = s.Address,
                            ExpiryDate = p.ExpiryDate,
                            DateTimeAdded = c.DateTimeCreated,
                            SellerImage = _base64Resizer.ResizeImage(_filePath.UserImage(s.ProfileImage)),
                            ProductImage = _base64Resizer.ResizeImage(_filePath.ProductImage(productImage.pf.Image))

                        }).ToList();
            if (!string.IsNullOrWhiteSpace(search))
            {
                data = data.Where(x => x.ProductName.Contains(search)).ToList();
                return data;
            }
            return data;
        }

        // ADD TO CART
        public bool AddCart(CartDTO_POST dto)
        {
            int success = 0;

            var checkProduct = _context.Carts.FirstOrDefault(x => x.BuyerId == dto.BuyerId && x.ProductId == dto.ProductId);
            if(checkProduct != null)
            {
                checkProduct.Quantity = checkProduct.Quantity + 1;
                _context.Carts.Update(checkProduct);
                success = _context.SaveChanges();

                if (success > 0)
                {
                    return true;
                }
                return false;
            }
            else
            {
                var data = new Cart();

                data.CartId = Guid.NewGuid();
                data.BuyerId = dto.BuyerId;
                data.ProductId = dto.ProductId;
                data.Quantity = dto.Quantity;
                data.DateTimeCreated = DateTime.Now;

                _context.Carts.Add(data);
                success = _context.SaveChanges();

                if (success > 0)
                {
                    return true;
                }
                return false;
            }
            
        }

        // DELETE CART
        public bool DeleteCart(Guid cartId)
        {
            int success = 0;
            var data = _context.Carts.FirstOrDefault(x => x.CartId == cartId);
            
            _context.Carts.Remove(data);
            success = _context.SaveChanges();
            
            if(success > 0)
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
