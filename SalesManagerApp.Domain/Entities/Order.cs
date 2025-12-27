using SalesManagerApp.Domain.Enums;

namespace SalesManagerApp.Domain.Entities
{
    public class Order : BaseEntity
    {
        public DateTime OrderDate { get; set; }

        public decimal TotalValue { get; set; }

        public OrderStatus Status { get; set; }


        #region Relacionamentos

        public Guid CustomerId { get; set; }

        public Customer? Customer { get; set; }

        public List<OrderItem>? OrderItems { get; set; }

        #endregion
    }
}
