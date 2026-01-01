using SalesManagerApp.Domain.Entities;

namespace SalesManagerApp.Domain.Interfaces.Repositories
{
    public interface IOrderRepository : IBaseRepository<Order>
    {
        ICollection<Order> GetPaginatedList(int pageNumber, int pageSize);
    }
}
