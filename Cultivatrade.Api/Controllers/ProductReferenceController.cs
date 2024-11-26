using Cultivatrade.Api.DTO;
using Cultivatrade.Api.Logics;
using Cultivatrade.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cultivatrade.Api.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class ProductReferenceController : Controller
    {
        private readonly ProductReferenceLogic _productReferenceLogic;
        public ProductReferenceController(ProductReferenceLogic productReferenceLogic)
        {
            _productReferenceLogic = productReferenceLogic;
        }

        [HttpGet]
        public IActionResult GetProductReference([FromQuery] string? search = "", [FromQuery] bool isVegetables = false, [FromQuery] bool isFruits = false)
        {
            try
            {
                var data = _productReferenceLogic.GetProductReference(search, isVegetables, isFruits);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + ex.StackTrace);
            }
        }

        [HttpPost]
        public IActionResult AddProductReference([FromForm] ProductReferenceDTO_POST dto)
        {
            try
            {
                bool isSuccess = _productReferenceLogic.AddProductReference(dto);
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


        [HttpGet("{productReferenceId}")]
        public IActionResult GetProductReferenceById(Guid productReferenceId)
        {
            var getFile = _productReferenceLogic.GetProductReferenceById(productReferenceId);
            if (getFile != null)
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

        [HttpPut("{productReferenceId}")]
        public IActionResult UpdateProductReference(Guid productReferenceId, [FromForm] ProductReferenceDTO_POST dto)
        {
            try
            {
                bool isSuccess = _productReferenceLogic.UpdateProductReference(productReferenceId, dto);
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

        [HttpDelete("{productReferenceId}")]
        public IActionResult DeleteProductReference(Guid productReferenceId)
        {
            try
            {
                bool isSuccess = _productReferenceLogic.DeleteProductReference(productReferenceId);
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
    }
}
