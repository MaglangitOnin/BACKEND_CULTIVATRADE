using Cultivatrade.Api.Logics;
using Microsoft.AspNetCore.Mvc;

namespace Cultivatrade.Api.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class PaymentController : Controller
    {
        private readonly PaymentLogic _paymentLogic;
        public PaymentController(PaymentLogic paymentLogic)
        {
            _paymentLogic = paymentLogic;
        }

        [HttpGet("{buyerId}")]
        public IActionResult GetPaymentByBuyerId(Guid buyerId)
        {
            try
            {
                var data = _paymentLogic.GetPaymentByBuyerId(buyerId);
                return Ok(data);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message + ex.StackTrace);
            }
        }
    }
}
