using SalesManagerApp.Domain.Entities;

namespace SalesManagerApp.Domain.Dtos.Requests
{
    public class AlterOrderRequestDto
    {
        public List<OrderItem>? OrderItems { get; set; }
    }
}
