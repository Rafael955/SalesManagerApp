using System;
using System.Collections.Generic;
using System.Text;

namespace SalesManagerApp.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string? Name { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        
        #region Relacionamentos

        public List<OrderItem>? OrderItems { get; set; }

        #endregion
    }
}
