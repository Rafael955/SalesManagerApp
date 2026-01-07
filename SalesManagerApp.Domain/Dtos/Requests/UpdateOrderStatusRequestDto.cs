using SalesManagerApp.Domain.Enums;

namespace SalesManagerApp.Domain.Dtos.Requests
{
    public class UpdateOrderStatusRequestDto
    {
        public OrderStatus Status { get; set; }
    }
}
