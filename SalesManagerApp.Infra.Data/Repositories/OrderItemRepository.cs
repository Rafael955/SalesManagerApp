using SalesManagerApp.Domain.Entities;
using SalesManagerApp.Domain.Interfaces.Repositories;

namespace SalesManagerApp.Infra.Data.Repositories
{
    public class OrderItemRepository : BaseRepository<OrderItem>, IOrderItemRepository
    {

    }
}
