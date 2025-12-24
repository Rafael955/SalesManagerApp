namespace SalesManagerApp.Domain.Dtos.Responses
{
    public class OrderItemResponseDto
    {
        public Guid Id { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public Guid? OrderId { get; set; }

        public string? ProductName { get; set; }
    }
}
