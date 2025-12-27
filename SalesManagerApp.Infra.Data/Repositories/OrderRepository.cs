using SalesManagerApp.Domain.Entities;
using SalesManagerApp.Domain.Interfaces.Repositories;
using SalesManagerApp.Infra.Data.Contexts;

namespace SalesManagerApp.Infra.Data.Repositories
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        public List<Order>? GetPaginatedList(int pageNumber, int pageSize)
        {
            using (var context = new DataContext())
            {
                return context.Set<Order>().OrderBy(o => o.OrderDate)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
            }
        }
    }
}
