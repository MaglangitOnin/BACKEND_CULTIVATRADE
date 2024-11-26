using Cultivatrade.Api.DTO;
using Cultivatrade.Api.Logics;
using Microsoft.AspNetCore.Mvc;
using VisitorSystem.Api.Logics;
using VisitorSystem.Api.Models;

namespace Cultivatrade.Api.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class NotificationController : Controller
    {
        private readonly NotificationLogic _notificationLogic;
        public NotificationController(NotificationLogic notificationLogic)
        {
            _notificationLogic = notificationLogic;
        }

        [HttpPost]
        public IActionResult AddNotification(NotificationDTO_POST dto)
        {
            try
            {
                bool isSuccess = _notificationLogic.AddNotification(dto);
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
        
        // BUYER
        [HttpGet("{buyerId}/Buyer")]
        public IActionResult GetNotificationByBuyerId(Guid buyerId, [FromQuery] PaginationRequest paginationRequest,  [FromQuery] bool isPaginated = false)
        {
            try
            {
                var data = _notificationLogic.GetNotificationByBuyerId(buyerId);
                if (isPaginated)
                {
                    var pagination = PaginationLogic.PaginateData(data, paginationRequest);
                    return Ok(pagination);
                }
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + ex.StackTrace);
            }
        }
        
        // SELLER
        [HttpGet("{userId}")]
        public IActionResult GetNotificationByUserId(Guid userId, [FromQuery] PaginationRequest paginationRequest, [FromQuery] bool isPaginated = true)
        {
            try
            {
                var data = _notificationLogic.GetNotificationByUserId(userId);
                if (isPaginated)
                {
                    var pagination = PaginationLogic.PaginateData(data, paginationRequest);
                    return Ok(pagination);
                }
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + ex.StackTrace);
            }
        }

        [HttpDelete("{notificationId}")]
        public IActionResult DeleteNotification(Guid notificationId)
        {
            try
            {
                bool isSuccess = _notificationLogic.DeleteNotification(notificationId);
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
