namespace SalesManagerApp.Domain.Entities
{
    public class Customer : BaseEntity
    {
        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public DateTime CreatedAt { get; set; }

        public List<Order>? Orders { get; set; }
    }
}
