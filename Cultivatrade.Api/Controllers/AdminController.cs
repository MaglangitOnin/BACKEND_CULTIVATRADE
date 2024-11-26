using Cultivatrade.Api.DatabaseConnection;
using Cultivatrade.Api.DTO;
using Cultivatrade.Api.Logics;
using Cultivatrade.Api.Models;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Serialization;
using BC = BCrypt.Net.BCrypt;
namespace Cultivatrade.Api.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class AdminController : Controller
    {
        private readonly AdminLogic _adminLogic;
        private readonly CultivatradeContext _context;
        
        public AdminController(AdminLogic adminLogic, CultivatradeContext context)
        {
            _adminLogic = adminLogic;
            _context = context;
        }

        [HttpPost("Login")]
        public IActionResult LoginAdmin(AdminDTO_LOGIN dto)
        {
            try
            {
                var data =  _adminLogic.LoginAdmin(dto);
                if(data != null)
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

        [HttpGet("Statistics")]
        public IActionResult GetStatistics()
        {
            try
            {
                var data = _adminLogic.GetStatistics();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + ex.StackTrace);
            }
        }

        [HttpGet("Product")]
        public IActionResult GetProductByDateTimeCreated([FromQuery] string? search = "", [FromQuery] int? year = 0, [FromQuery] int? month = 0)
        {
            try
            {
                var data = _adminLogic.GetProductByDateTimeCreated(search, year, month);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + ex.StackTrace);
            }
        }

        [HttpPut("{adminId}")]
        public IActionResult UpdatePassword(Guid adminId, AdminDTO_POST dto)
        {
            try
            {
                bool isSuccess = _adminLogic.UpdatePassword(adminId, dto);
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

        [HttpGet("Order")]
        public IActionResult GetSoldProduct([FromQuery] string? search = "", [FromQuery] int? year = 0, [FromQuery] int? month = 0)
        {
            try
            {
                var data = _adminLogic.GetSoldProduct(search, year, month);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + ex.StackTrace);
            }
        }
    }
}
