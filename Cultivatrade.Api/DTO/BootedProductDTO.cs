namespace Cultivatrade.Api.DTO
{
    public class BoostedProductDTO_POST
    {
        public Guid ProductId { get; set; }
        public float BoostCost { get; set; }
        public int NumberOfDays { get; set; }
    }

    public class BoostedProductDTO_GET
    {
        public int NumberOfDays { get; set; }
        public double BoostCost { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsExpired { get; set; }
    }
}
