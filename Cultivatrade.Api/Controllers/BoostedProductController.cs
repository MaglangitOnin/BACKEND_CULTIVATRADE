using Cultivatrade.Api.DTO;
using Cultivatrade.Api.Logics;
using Cultivatrade.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cultivatrade.Api.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class BoostedProductController : Controller
    {
        private readonly BoostedProductLogic _boostedProductLogic;
        public BoostedProductController(BoostedProductLogic boostedProductLogic)
        {
            _boostedProductLogic = boostedProductLogic;
        }

        [HttpGet("{productId}")]
        public IActionResult GetBoostProductByProductId(Guid productId)
        {
            try
            {
                var data = _boostedProductLogic.GetBoostProductByProductId(productId);
                return Ok(data);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message + ex.StackTrace);
            }
        }

        [HttpPost]
        public IActionResult AddBoostProduct(BoostedProductDTO_POST dto)
        {
            try
            {
                bool isSuccess = _boostedProductLogic.AddBoostProduct(dto);
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
