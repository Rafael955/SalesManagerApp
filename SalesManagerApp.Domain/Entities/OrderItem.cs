namespace SalesManagerApp.Domain.Entities
{
    public class OrderItem : BaseEntity
    {
        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }


        #region Relacionamentos

        public Guid OrderId { get; set; }

        public Order? Order { get; set; }

        public Guid ProductId { get; set; }

        public Product? Product { get; set; }

        #endregion
    }
}
