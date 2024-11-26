using Cultivatrade.Api.DTO;
using Cultivatrade.Api.Logics;
using Microsoft.AspNetCore.Mvc;

namespace Cultivatrade.Api.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class ReportController : Controller
    {
        private readonly ReportLogic _reportLogic;
        public ReportController(ReportLogic reportLogic)
        {
            _reportLogic = reportLogic;
        }

        [HttpGet("User/{buyerId}/Product/{productId}")]
        public IActionResult IsAlreadyReported(Guid buyerId, Guid productId)
        {
            try
            {
                bool isExists = _reportLogic.IsAlreadyReported(buyerId, productId);
                if (isExists)
                {
                    return Json("error");
                }
                return Ok("success");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message + ex.StackTrace);
            }
        }

        [HttpGet("{productId}")]
        public IActionResult GetReportByProductId(Guid productId)
        {
            try
            {
                var data = _reportLogic.GetReportByProductId(productId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + ex.StackTrace);
            }
        }

        [HttpPost]
        public IActionResult AddReport(ReportDTO_POST dto)
        {
            try
            {
                bool isSuccess = _reportLogic.AddReport(dto);
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
