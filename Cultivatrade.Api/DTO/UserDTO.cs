using System.ComponentModel.DataAnnotations;

namespace Cultivatrade.Api.DTO
{
    public class UserDTO_LOGIN
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class UserDTO_GET
    {
        public Guid UserId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public bool IsSeller { get; set; }
        public bool IsApproved { get; set; }
        public string? ProfileDataUrl { get; set; }
        public string? BusinessPermitNumber { get; set; }
        public string? BusinessPermitDataUrl { get; set; }
        public string? SanitaryPermitDataUrl { get; set; }
    }

    public class UserDTO_POST
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public IFormFile? ProfileImage { get; set; }
    }

    public class UserDTO_PUT
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Phone { get; set; }
        public string? Password { get; set; }
        public string Address { get; set; }
        public IFormFile? ProfileImage { get; set; }
        
    }

    public class UserDTO_PATCH
    {
        //public string BusinessPermitNumber { get; set; }
        public IFormFile BusinessPermitImage { get; set; }
        //public IFormFile? SanitaryPermitImage { get; set; }
        public string FarmName { get; set; }
        public string FarmAddress { get; set; }
        public string FarmDescription { get; set; }
    }
}
