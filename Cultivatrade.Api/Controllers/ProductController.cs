using Cultivatrade.Api.DatabaseConnection;
using Cultivatrade.Api.DTO;
using Cultivatrade.Api.Logics;
using Cultivatrade.Api.Models;
using Microsoft.AspNetCore.Mvc;
using VisitorSystem.Api.Logics;
using VisitorSystem.Api.Models;

namespace Cultivatrade.Api.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class ProductController : Controller
    {
        private readonly ProductLogic _productLogic;
        private readonly CultivatradeContext _context;
        public ProductController(ProductLogic productLogic, CultivatradeContext context)
        {
            _productLogic = productLogic;
            _context = context;
        }

        // GET PRODUCT
        [HttpGet]
        public IActionResult GetProduct([FromQuery] Guid userId, [FromQuery] PaginationRequest? paginationRequest, [FromQuery] string? search = "", [FromQuery] bool? isPaginated = true)
        {
            try
            {
                var data = _productLogic.GetProduct(userId, search);
              
                var pagination = PaginationLogic.PaginateData(data, paginationRequest);
                return Ok(pagination);
                
               
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message + ex.StackTrace);
            }
        }

        // GET PRODUCT BY USER ID
        [HttpGet("User/{userId}")]
        public IActionResult GetProductByUserId(Guid userId, [FromQuery] PaginationRequest? paginationRequest, [FromQuery] string? search = "")
        {
            try
            {
                var data = _productLogic.GetProductByUserId(userId, search);
                var pagination = PaginationLogic.PaginateData(data, paginationRequest);
                return Ok(pagination);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + ex.StackTrace);
            }
        }

        // GET PRODUCT BY PRODUCT ID
        [HttpGet("{productId}")]
        public IActionResult GetProductByProductId(Guid productId)
        {
            try
            {
                var data = _productLogic.GetProductByProductId(productId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + ex.StackTrace);
            }
        }

        // ADD PRODUCT
        [HttpPost]
        public IActionResult AddProduct(ProductDTO_POST dto)
        {
            try
            {
                bool isSuccess = _productLogic.AddProduct(dto);
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
        
        [HttpPut("{productId}")]
        public IActionResult AddProduct(Guid productId, ProductDTO_PUT dto)
        {
            try
            {
                bool isSuccess = _productLogic.UpdateProduct(productId, dto);
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
        
        [HttpDelete("{productId}")]
        public IActionResult DeleteProduct(Guid productId)
        {
            try
            {
                bool isSuccess = _productLogic.DeleteProduct(productId);
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
        
        [HttpDelete("{productId}/User")]
        public IActionResult DisableProduct(Guid productId)
        {
            try
            {
                bool isSuccess = _productLogic.DisableProduct(productId);
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

        [HttpGet("Count")]
        public IActionResult GetProduct()
        {
            try
            {
                var data = _productLogic.GetProduct();
                return Ok(data);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message + ex.StackTrace);
            }
        }

        [HttpGet("{productId}/Quantity")]
        public IActionResult GetProductQuantity(Guid productId)
        {
            try
            {
                var data = _context.Products.FirstOrDefault(x => x.ProductId == productId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + ex.StackTrace);
            }
        }

        [HttpPut("{productId}/Quantity/{quantity}")]
        public IActionResult UpdateProductQuantity(Guid productId, int quantity, [FromQuery] bool isInventory = false)
        {
            try
            {
                    bool isSuccess = _productLogic.UpdateProductQuantity(productId, quantity, isInventory);
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
