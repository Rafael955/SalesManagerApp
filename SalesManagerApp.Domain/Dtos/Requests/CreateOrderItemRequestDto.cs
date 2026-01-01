namespace SalesManagerApp.Domain.Dtos.Requests
{
    public class CreateOrderItemRequestDto
    {
        public int Quantity { get; set; }

        public Guid? ProductId { get; set; }
    }
}
