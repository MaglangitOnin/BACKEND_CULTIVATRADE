namespace Cultivatrade.Api.DTO
{
    public class AdminDTO_LOGIN
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class AdminDTO_POST
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
    }
}
