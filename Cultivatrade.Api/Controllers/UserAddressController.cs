using Cultivatrade.Api.DTO;
using Cultivatrade.Api.Logics;
using Microsoft.AspNetCore.Mvc;

namespace Cultivatrade.Api.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class UserAddressController : Controller
    {
        private readonly UserAddressLogic _userAddressLogic;
        public UserAddressController(UserAddressLogic userAddressLogic)
        {
            _userAddressLogic = userAddressLogic;
        }

        [HttpGet("{userId}")]
        public IActionResult GetUserAddress(Guid userId)
        {
            try
            {
                var data = _userAddressLogic.GetUserAddress(userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + ex.StackTrace);
            }
        }

        [HttpPost]
        public IActionResult AddUserAddress(UserAddressDTO_POST dto)
        {
            try
            {
                bool isSuccess = _userAddressLogic.AddUserAddress(dto);
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
        
        [HttpPut("{userAddressId}")]
        public IActionResult UpdateUserAddress(Guid userAddressId, UserAddressDTO_PUT dto)
        {
            try
            {
                bool isSuccess = _userAddressLogic.UpdateUserAddress(userAddressId, dto);
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

        [HttpDelete("{userAddressId}")]
        public IActionResult DeleteUserAddress(Guid userAddressId)
        {
            try
            {
                bool isSuccess = _userAddressLogic.DeleteUserAddress(userAddressId);
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
