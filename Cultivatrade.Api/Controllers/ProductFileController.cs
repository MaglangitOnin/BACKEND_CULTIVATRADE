using Cultivatrade.Api.DTO;
using Cultivatrade.Api.Logics;
using Microsoft.AspNetCore.Mvc;

namespace Cultivatrade.Api.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class ProductFileController : Controller
    {
        private readonly ProductFileLogic _productFileLogic;
        public ProductFileController(ProductFileLogic productFileLogic)
        {
            _productFileLogic = productFileLogic;
        }

        [HttpGet("{productFileId}")]
        public IActionResult GetProductFile(Guid productFileId)
        {
            var getFile = _productFileLogic.GetProductFileByProductFileId(productFileId);
            if(getFile != null)
            {
                var result = new FileStreamResult(getFile.FileStream, getFile.ContentType)
                {
                    FileDownloadName = getFile.FileName
                };
                Response.Headers.Add("Content-Disposition", $"attachment; filename=\"{getFile.FileName}\"");

                return result;
            }
            return Json("Image not found");
        }

        [HttpGet("Product/{productId}")]
        public IActionResult GetProductFileByProductId(Guid productId)
        {
            try
            {
                var data = _productFileLogic.GetProductFileByProductId(productId);
                return Ok(data);
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message + ex.StackTrace);
            }
        }

        [HttpPost]
        public IActionResult AddProductFile([FromForm] ProductFileDTO_POST dto)
        {
            try
            {
                bool isSuccess = _productFileLogic.AddProductFile(dto);
                if (isSuccess)
                {
                    return Ok("success");
                }
                return Json("error");
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message + ex.StackTrace);
            }
        }

        [HttpPut("Product/{productId}")]
        public IActionResult UpdateProduct(Guid productId, [FromForm] ProductFileDTO_POST dto) 
        {
            try
            {
                bool isSuccess = _productFileLogic.UpdateProductFile(productId, dto);
                if (isSuccess)
                {
                    return Ok("success");
                }
                return Json("error");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + ex.StackTrace);
            }
        }

        [HttpDelete("{productFileId}")]
        public IActionResult DeleteProductFile(Guid productFileId) 
        {
            try
            {
                bool isSuccess = _productFileLogic.DeleteProductFile(productFileId);
                if (isSuccess)
                {
                    return Ok("success");
                }
                return Json("error");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message + ex.StackTrace);
            }
        }
    }
}
