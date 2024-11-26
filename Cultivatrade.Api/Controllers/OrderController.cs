using Cultivatrade.Api.DTO;
using Cultivatrade.Api.Logics;
using Microsoft.AspNetCore.Mvc;
using VisitorSystem.Api.Logics;
using VisitorSystem.Api.Models;

namespace Cultivatrade.Api.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class OrderController : Controller
    {
        private readonly OrderLogic _orderLogic;
        public OrderController(OrderLogic orderLogic)
        {
            _orderLogic = orderLogic;
        }

        [HttpGet("Inventory/User/{sellerId}")]
        public IActionResult GetInventory(Guid sellerId, [FromQuery] PaginationRequest paginationRequest, [FromQuery] string? search = "")
        {
            try
            {
                var data = _orderLogic.GetInventory(sellerId, search);
                var pagination = PaginationLogic.PaginateData(data, paginationRequest);
                return Ok(pagination);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + ex.StackTrace);
            }
        }

        [HttpGet]
        public IActionResult GetOrder([FromQuery] PaginationRequest paginationRequest, [FromQuery] Guid buyerId , [FromQuery] Guid sellerId, [FromQuery] string? search = "") 
        {
            try
            {
                var data = _orderLogic.GetOrder(buyerId, sellerId, search);
                var pagination = PaginationLogic.PaginateData(data, paginationRequest);
                return Ok(pagination);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message + ex.StackTrace);
            }
        }

        [HttpPost("Multiple")]
        public IActionResult AddMultipleOrder(List<OrderDTO_POST> dto)
        {
            try
            {
                var isSuccess = _orderLogic.AddMultipleOrder(dto);
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

        [HttpPost]
        public IActionResult AddOrder(OrderDTO_POST dto) 
        {
            try
            {
                var data = _orderLogic.AddOrder(dto);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + ex.StackTrace);
            }
        }

        [HttpPut("{orderId}")]
        public IActionResult UpdateOrderStatus(Guid orderId, OrderDTO_PUT dto)
        {
            try
            {
                bool isSuccess = _orderLogic.UpdateOrderStatus(orderId, dto);
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
