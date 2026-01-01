using Microsoft.EntityFrameworkCore;
using SalesManagerApp.Domain.Entities;
using SalesManagerApp.Domain.Interfaces.Repositories;
using SalesManagerApp.Infra.Data.Contexts;

namespace SalesManagerApp.Infra.Data.Repositories
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        public ICollection<Order> GetPaginatedList(int pageNumber, int pageSize)
        {
            using (var context = new DataContext())
            {
                return context.Set<Order>().OrderBy(o => o.OrderDate)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
            }
        }

        public override Order? GetById(Guid id)
        {
            using (var context = new DataContext())
            {
                return context.Set<Order>()
                    .Include(o => o.Customer)
                    .Include(o => o.OrderItems)!
                        .ThenInclude(oi => oi.Product)
                    .SingleOrDefault(o => o.Id == id);
            }
        }
    }
}
