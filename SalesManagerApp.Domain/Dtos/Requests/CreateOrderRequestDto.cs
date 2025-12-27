using SalesManagerApp.Domain.Entities;

namespace SalesManagerApp.Domain.Dtos.Requests
{
    public class CreateOrderRequestDto
    {
        public DateTime OrderDate { get; set; }

        public decimal TotalValue { get; set; }

        public List<OrderItem>? OrderItems { get; set; }
    }
}
