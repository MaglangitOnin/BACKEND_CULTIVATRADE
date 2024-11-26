using Cultivatrade.Api.DTO;
using Cultivatrade.Api.Logics;
using Microsoft.AspNetCore.Mvc;

namespace Cultivatrade.Api.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class MessageController : Controller
    {
        public readonly MessageLogic _messageLogic;
        public MessageController(MessageLogic messageLogic)
        {
            _messageLogic = messageLogic;
        }

        [HttpGet("Buyer/{buyerId}/Seller/{sellerId}")]
        public IActionResult GetMessages(Guid buyerId, Guid sellerId)
        {
            try
            {
                var data = _messageLogic.GetMessages(buyerId, sellerId);
                return Ok(data);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message + ex.StackTrace);
            }
        }

        [HttpGet]
        public IActionResult GetContact([FromQuery] Guid buyerId, [FromQuery] Guid sellerId)
        {
            try
            {
                var data = _messageLogic.GetContact(buyerId, sellerId);
                return Ok(data);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message + ex.StackTrace);
            }
        }
        
        [HttpPost]
        public IActionResult AddMessage(MessageDTO_POST dto)
        {
            try
            {
                var data = _messageLogic.AddMessage(dto);
                return Ok(data);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message + ex.StackTrace);
            }
        }
    }
}
