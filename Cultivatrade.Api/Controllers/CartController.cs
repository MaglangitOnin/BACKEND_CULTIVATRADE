using Cultivatrade.Api.DatabaseConnection;
using Cultivatrade.Api.DTO;
using Cultivatrade.Api.Logics;
using Microsoft.AspNetCore.Mvc;
using VisitorSystem.Api.Logics;
using VisitorSystem.Api.Models;

namespace Cultivatrade.Api.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class CartController : Controller
    {
        private readonly CartLogic _cartLogic;

        public CartController(CartLogic cartLogic)
        {
            _cartLogic = cartLogic;
        }

        [HttpPut("{cartId}/Quantity/{quantityBought}")]
        public IActionResult UpdateQuanityBought(Guid cartId, int quantityBought)
        {
            try
            {
                _cartLogic.UpdateQuanityBought(cartId, quantityBought);
                return Ok("success");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + ex.StackTrace);
            }
        }

        // GET CART BY USER ID
        [HttpGet("{userId}")]
        public IActionResult GetCartByUserId(Guid userId, [FromQuery] PaginationRequest paginationRequest, [FromQuery] string? search = "")
        {
            try
            {
                var data = _cartLogic.GetCartByUserId(userId, search);
                var pagination = PaginationLogic.PaginateData(data, paginationRequest);
                return Ok(pagination);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + ex.StackTrace);
            }
        }

        // ADD TO CART
        [HttpPost]
        public IActionResult AddCart(CartDTO_POST dto)
        {
            try
            {
                bool isSuccess = _cartLogic.AddCart(dto);
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

        [HttpDelete("{cartId}")]
        public IActionResult DeleteCart(Guid cartId)
        {
            try
            {
                bool isSuccess = _cartLogic.DeleteCart(cartId);
                if(isSuccess) 
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
