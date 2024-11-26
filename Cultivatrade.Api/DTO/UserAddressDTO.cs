namespace Cultivatrade.Api.DTO
{
    public class UserAddressDTO_POST
    {
        public Guid UserId { get; set; }
        public string Address { get; set; }
    }

    public class UserAddressDTO_PUT
    {
        public string Address { get; set; }
    }

    public class UserAddressDTO_GET
    {
        public Guid UserAddressId { get; set; }
        public Guid UserId { get; set;}
        public string Address { get; set; }
        public DateTime DateTimeCreated { get; set; }
    }
}
