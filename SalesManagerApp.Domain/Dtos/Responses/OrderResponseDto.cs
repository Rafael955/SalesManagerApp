namespace SalesManagerApp.Domain.Dtos.Responses
{
    public class OrderResponseDto
    {
        public Guid Id { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal TotalValue { get; set; }

        public string OrderStatus { get; set; }

        public Guid? CustomerId { get; set; }

        public CustomerResponseDto? Customer { get; set; }

        public List<OrderItemResponseDto>? OrderItems { get; set; }
    }
}
