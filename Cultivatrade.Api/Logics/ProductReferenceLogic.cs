using Cultivatrade.Api.DatabaseConnection;
using Cultivatrade.Api.DTO;
using Cultivatrade.Api.Models;
using Cultivatrade.Api.Services;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp;
using ImageSharpImage = SixLabors.ImageSharp.Image;
using FileUpload.Api.Logics;
using System.Drawing.Imaging;
using System.Drawing;


namespace Cultivatrade.Api.Logics
{
    public class ProductReferenceLogic
    {
        private readonly CultivatradeContext _context;
        private readonly Base64Resizer _base64Resizer;
        private readonly FilePath _filePath;
        private readonly CheckFileType _checkFileType;
        public ProductReferenceLogic(CultivatradeContext context, Base64Resizer base64Resizer, FilePath filePath, CheckFileType checkFileType)
        {
            _context = context;
            _base64Resizer = base64Resizer;
            _filePath = filePath;
            _checkFileType = checkFileType;
        }

        public bool AddProductReference(ProductReferenceDTO_POST dto)
        {
            int success = 0;
            string profileName = "";
            Guid productReferenceId = Guid.NewGuid();
            DateTime dateTimeCreated = DateTime.Now;

            if (dto.File != null)
            {
                profileName = productReferenceId.ToString() + Path.GetExtension(dto.File.FileName);
                var profilePath = _filePath.ProductImage("");
                string profileFilePath = Path.Combine(profilePath, profileName);

                using (var image = ImageSharpImage.Load(dto.File.OpenReadStream()))
                {
                    image.Mutate(x => x.Resize(200, 200));
                    image.Save(profileFilePath);
                }
            }

            var data = new ProductReference();
            data.ProductReferenceId = productReferenceId;
            data.ProductName = dto.Name;
            data.ProductImage = profileName;
            data.Price = dto.Price;
            data.DateTimeCreated = dateTimeCreated;
            data.CategoryName = dto.CategoryName;

            _context.ProductReferences.Add(data);
            success = _context.SaveChanges();

            if (success > 0)
            {
                return true;
            }
            return false;
        }

        public List<ProductReferenceDTO_GET> GetProductReference(string? search = "", bool isVegetable = false, bool isFruits = false)
        {
            var data = (from pr in _context.ProductReferences
                        select new ProductReferenceDTO_GET
                        {
                            ProductReferenceId = pr.ProductReferenceId,
                            Name = pr.ProductName,
                            Price = pr.Price,
                            DateTimeCreated = pr.DateTimeCreated,
                            CategoryName = pr.CategoryName,
                            ProductImage = _base64Resizer.ResizeImage(_filePath.ProductImage(pr.ProductImage)),
                        }).ToList();
            if (!string.IsNullOrWhiteSpace(search))
            {
                data = data.Where(x => x.Name.Contains(search)).ToList();
            }

            if (isVegetable)
            {
               data = (from pr in _context.ProductReferences
                                 where pr.CategoryName == "Vegetables"
                                 select new ProductReferenceDTO_GET
                                 {
                                     ProductReferenceId = pr.ProductReferenceId,
                                     Name = pr.ProductName,
                                     Price = pr.Price,
                                     DateTimeCreated = pr.DateTimeCreated,
                                     CategoryName = pr.CategoryName,
                                     ProductImage = _base64Resizer.ResizeImage(_filePath.ProductImage(pr.ProductImage)),
                                 }).ToList();
                return data;
            }

            if (isFruits)
            {
                data = (from pr in _context.ProductReferences
                        where pr.CategoryName == "Fruits"
                        select new ProductReferenceDTO_GET
                        {
                            ProductReferenceId = pr.ProductReferenceId,
                            Name = pr.ProductName,
                            Price = pr.Price,
                            DateTimeCreated = pr.DateTimeCreated,
                            CategoryName = pr.CategoryName,
                            ProductImage = _base64Resizer.ResizeImage(_filePath.ProductImage(pr.ProductImage)),
                        }).ToList();
                return data;
            }

            return data;
        }

        public bool DeleteProductReference(Guid productReferenceId)
        {
            int success = 0;
            var data = _context.ProductReferences.FirstOrDefault(x => x.ProductReferenceId == productReferenceId);

            _context.ProductReferences.Remove(data);
            success = _context.SaveChanges();

            return success > 0;
        }

        public DisplayFile GetProductReferenceById(Guid productReferenceId)
        {
            var data = _context.ProductReferences.FirstOrDefault(x => x.ProductReferenceId == productReferenceId);
            if (data != null)
            {
                var displayFile = new DisplayFile();
                var checkFile = _filePath.ProductImage(data.ProductImage);
                if (string.IsNullOrEmpty(checkFile) || !File.Exists(checkFile))
                {
                    return null;
                }
                var fileStream = new FileStream(_filePath.ProductImage(data.ProductImage), FileMode.Open, FileAccess.Read);

                var getFileExtension = Path.GetExtension(_filePath.ProductImage(data.ProductImage));
                var getContentType = _checkFileType.GetContentType(getFileExtension);
                displayFile.FileStream = fileStream;
                displayFile.ContentType = getContentType;
                displayFile.FileName = data.ProductImage;

                return displayFile;
            }
            return null;
        }

        public bool UpdateProductReference(Guid productReferenceId, ProductReferenceDTO_POST dto)
        {
            int success = 0;
            string profileName = "";

            var data = _context.ProductReferences.FirstOrDefault(x => x.ProductReferenceId == productReferenceId);

            // PROFILE
            if (dto.File != null && dto.File.Length > 0)
            {
                profileName = productReferenceId.ToString() + Path.GetExtension(dto.File.FileName);
                data.ProductImage = profileName;
                var profilePath = _filePath.ProductImage("");
                string profileFilePath = Path.Combine(profilePath, profileName);

                using (var image = System.Drawing.Image.FromStream(dto.File.OpenReadStream()))
                using (var resizedImage = new Bitmap(200, 200))
                using (var graphics = Graphics.FromImage(resizedImage))
                {
                    graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    graphics.DrawImage(image, 0, 0, 200, 200);
                    resizedImage.Save(profileFilePath, ImageFormat.Png);
                }
            }

            data.CategoryName = dto.CategoryName;
            data.ProductName = dto.Name;
            data.Price = dto.Price;

            _context.ProductReferences.Update(data);
            success = _context.SaveChanges();

            if (success > 0)
            {
                return true;
            }
            return false;
        }
    }
}
