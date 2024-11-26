using Cultivatrade.Api.DatabaseConnection;
using Cultivatrade.Api.DTO;
using Cultivatrade.Api.Logics;
using Microsoft.AspNetCore.Mvc;

namespace Cultivatrade.Api.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly UserLogic _userLogic;

        public UserController(UserLogic userLogic)
        {
            _userLogic = userLogic;
        }

        // RESET PASSWORD
        [HttpPut("Email/{email}/Password/{password}")]
        public IActionResult ResetPassword(string email, string password)
        {
            try
            {
                bool isSuccess = _userLogic.ResetPassword(email, password);

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
        
        // VERIFY CODE
        [HttpGet("Email/{email}/VerificationCode/{verificationCode}")]
        public IActionResult VerifyCode(string email, int verificationCode)
        {
            try
            {
                var data = _userLogic.VerifyCode(email, verificationCode);

                if (data != null)
                {
                    return Ok(data);
                }
                return Json("error");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + ex.StackTrace);
            }
        }
        
        // SEND CODE
        [HttpGet("Email/{email}")]
        public IActionResult SendCode(string email)
        {
            try
            {
                var data = _userLogic.SendCode(email);

                if (data != null)
                {
                    return Ok(data);
                }
                return Json("error");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + ex.StackTrace);
            }
        }
        
        [HttpGet("Email")]
        public IActionResult SendRegistrationCode([FromQuery] string email, [FromQuery] int verificationCode)
        {
            try
            {
                var data = _userLogic.SendRegistrationCode(email, verificationCode);

                if (data == null)
                {
                    return Ok(data);
                }
                return Json("error");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + ex.StackTrace);
            }
        }

        // LOGIN USER
        [HttpGet("Email/{email}/Password/{password}")]
        public IActionResult LoginUser(string email, string password)
        {
            try
            {
                var data = _userLogic.LoginUser(email, password);

                if (data != null)
                {
                    return Ok(data);
                }
                return Json("error");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + ex.StackTrace);
            }
        }

        // GET USER BY USER ID
        [HttpGet("{userId}")]
        public IActionResult GetUserByUserId(Guid userId)
        {
            try
            {
                var data = _userLogic.GetUserByUserId(userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + ex.StackTrace);
            }
        }

        // ADD USER
        [HttpPost]
        public IActionResult AddUser([FromForm] UserDTO_POST dto)
        {
            try 
            {
                var checkUser = _userLogic.CheckUserByEmail(dto.Email);
                if(checkUser == null)
                {
                    bool isSuccess = _userLogic.AddUser(dto);
                    if (isSuccess)
                    {
                        return Ok("success");
                    }
                    return Json("error");
                }
                return Json("exists");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message + ex.StackTrace);
            }
        }

        // UPDATE USER
        [HttpPut("{userId}")]
        public IActionResult UpdateUser(Guid userId, [FromForm] UserDTO_PUT dto)
        {
            try
            {
                bool isSuccess = _userLogic.UpdateUser(userId, dto);
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

        // SUBMIT SELLER REQUIREMENTS
        [HttpPut("{userId}/Requirements")]
        public IActionResult SubmitSellerRequirements(Guid userId, [FromForm] UserDTO_PATCH dto)
        {
            try
            {
                bool isSuccess = _userLogic.SubmitSellerRequirements(userId, dto);
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

        // APPROVE USER AS SELLER
        [HttpPut("{userId}/ApproveDisapprove")]
        public IActionResult ApproveDisapprove(Guid userId)
        {
            try
            {
                bool isSuccess = _userLogic.ApproveDisapprove(userId);
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

        // GET SELLER
        [HttpGet("Seller")]
        public IActionResult GetSeller()
        {
            try
            {
                var data = _userLogic.GetSeller();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + ex.StackTrace);
            }
        }
        // GET BUYER
        [HttpGet("Buyer")]
        public IActionResult GetBuyer()
        {
            try
            {
                var data = _userLogic.GetBuyer();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + ex.StackTrace);
            }
        }

        [HttpGet("AllBuyers")]
        public IActionResult GetAllBuyers()
        {
            try
            {
                var data = _userLogic.GetAllBuyers();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + ex.StackTrace);
            }
        }
        // GET USER BY STATUS
        [HttpGet("{isApproved}/Status")]
        public IActionResult GetUserByUserStatus(bool isApproved)
        {
            try
            {
                var data = _userLogic.GetUserByStatus(isApproved);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + ex.StackTrace);
            }
        }
    }
}
