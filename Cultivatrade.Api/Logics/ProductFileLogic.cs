using Cultivatrade.Api.DatabaseConnection;
using Cultivatrade.Api.DTO;
using Cultivatrade.Api.Models;
using Cultivatrade.Api.Services;
using FileUpload.Api.Logics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.IIS.Core;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.Drawing;
using System.Drawing.Imaging;

namespace Cultivatrade.Api.Logics
{
    public class ProductFileLogic
    {
        private readonly CultivatradeContext _context;
        private readonly CheckFileType _checkFileType;
        private readonly Base64Resizer _base64Resizer;
        private readonly FilePath _filePath;
        public ProductFileLogic(CultivatradeContext context, CheckFileType checkFileType, Base64Resizer base64Resizer, FilePath filePath)
        {
            _context = context;
            _checkFileType = checkFileType;
            _base64Resizer = base64Resizer;
            _filePath = filePath;
        }

        // GET PRODUCT FILE BY PRODUCT ID
        public List<ProductFileDTO_GET> GetProductFileByProductId(Guid productId)
        {
            var data = (from pf in _context.ProductFiles
                        where pf.ProductId == productId
                        select new ProductFileDTO_GET
                        {
                            ProductFileId = pf.ProductFileId,
                            ProductPath = pf.ImagePath,
                            ProductImage = _base64Resizer.ResizeImage(_filePath.ProductImage(pf.Image)),
                        }).ToList();

            return data;
        }

        // ADD PRODUCT FILE
        public bool AddProductFile(ProductFileDTO_POST dto)
        {
            int success = 0;
            foreach (var file in dto.Files)
            {
                Guid fileId = Guid.NewGuid();
                string fileName = fileId + Path.GetExtension(file.FileName);
                var path = _filePath.ProductImage("");
                string filePath = Path.Combine(path, fileName);

                using (var image = SixLabors.ImageSharp.Image.Load(file.OpenReadStream()))
                {
                  
                    image.Mutate(x => x.Resize(200, 200)); 

                  
                    image.Save(filePath); 
                }

                var data = new ProductFile
                {
                    ProductFileId = fileId,
                    ProductId = dto.ProductId,
                    Image = fileName,
                    ImagePath = filePath,
                    DateTimeCreated = DateTime.Now
                };

                _context.ProductFiles.Add(data);
            }

            success = _context.SaveChanges();
            return success > 0;
        }


        // GET PRODUCT FILE BY PRODUCT ID
        public DisplayFile GetProductFileByProductFileId(Guid productFileId)
        {
            var data = _context.ProductFiles.FirstOrDefault(x => x.ProductFileId == productFileId);
            if (data != null)
            {
                var displayFile = new DisplayFile();
                var checkFile = _filePath.ProductImage(data.Image);
                if (string.IsNullOrEmpty(checkFile) || !File.Exists(checkFile))
                {
                    return null;
                }
                var fileStream = new FileStream(_filePath.ProductImage(data.Image), FileMode.Open, FileAccess.Read);
                
                var getFileExtension = Path.GetExtension(_filePath.ProductImage(data.Image));
                var getContentType = _checkFileType.GetContentType(getFileExtension);
                displayFile.FileStream = fileStream;
                displayFile.ContentType = getContentType;
                displayFile.FileName = data.Image;

                return displayFile;
            }
            return null;
           
        }

        public bool UpdateProductFile(Guid productId, ProductFileDTO_POST dto)
        {
            int success = 0;
            var data = _context.ProductFiles.Where(x => x.ProductId == productId).ToList();
            _context.ProductFiles.RemoveRange(data);

            success = _context.SaveChanges();

            if(success > 0)
            {
                foreach(var pf in data)
                {
                    if (File.Exists(_filePath.ProductImage(pf.Image)))
                    {
                        File.Delete(_filePath.ProductImage(pf.Image));
                    }
                }
                AddProductFile(dto);
                return true;
            }
            return false;
        }

        public bool DeleteProductFile(Guid productFileId)
        {
            int success = 0;
            
            var data = _context.ProductFiles.FirstOrDefault(x => x.ProductFileId == productFileId);

            _context.ProductFiles.Remove(data);
            success = _context.SaveChanges();

            if(success > 0)
            {
                return true;
            }
            return false;
        }
    }
}
