namespace SalesManagerApp.Domain.Dtos.Requests
{
    public class OrderItemRequestDto
    {
        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public Guid? OrderId { get; set; }

        public Guid? ProductId { get; set; }
    }
}
