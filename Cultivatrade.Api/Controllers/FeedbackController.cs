using Cultivatrade.Api.DTO;
using Cultivatrade.Api.Logics;
using Microsoft.AspNetCore.Mvc;

namespace Cultivatrade.Api.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class FeedbackController : Controller
    {
        private readonly FeedbackLogic _feedbackLogic;
        public FeedbackController(FeedbackLogic feedbackLogic)
        {
            _feedbackLogic = feedbackLogic;
        }

        //[HttpGet("{productId}")]
        //public IActionResult GetFeedbackByProductId(Guid productId)
        //{
        //    try
        //    {
        //        var data = _feedbackLogic.GetFeedbackByProductId(productId);
        //        return Ok(data);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message + ex.StackTrace);
        //    }
        //}

        [HttpGet("User/{userId}/Product/{productId}")]
        public IActionResult IsAlreadyRated(Guid userId, Guid productId)
        {
            try
            {
                bool isTrue = _feedbackLogic.IsAlreadyRated(userId, productId);
                if (isTrue)
                {
                    return Ok("error");
                }
                return Json("success");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + ex.StackTrace);
            }
        }

        // ADD FEEDBACK
        [HttpPost]
        public IActionResult AddFeeback(FeedbackDTO_POST dto)
        {
            try
            {
                bool isSuccess = _feedbackLogic.AddFeedback(dto);
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
