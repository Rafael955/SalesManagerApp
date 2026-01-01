namespace SalesManagerApp.Domain.Dtos.Requests
{
    public class CreateOrderRequestDto
    {
        public Guid CustomerId { get; set; }

        public List<CreateOrderItemRequestDto>? OrderItems { get; set; }
    }
}
