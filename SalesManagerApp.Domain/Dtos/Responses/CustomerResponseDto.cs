using SalesManagerApp.Domain.Entities;

namespace SalesManagerApp.Domain.Dtos.Responses
{
    public class CustomerResponseDto
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public ICollection<Order>? Orders { get; set; }
    }
}
