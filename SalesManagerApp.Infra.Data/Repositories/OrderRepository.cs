using Microsoft.EntityFrameworkCore;
using SalesManagerApp.Domain.Entities;
using SalesManagerApp.Domain.Interfaces.Repositories;
using SalesManagerApp.Infra.Data.Contexts;

namespace SalesManagerApp.Infra.Data.Repositories
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        private readonly DataContext context;

        public OrderRepository(DataContext context) : base(context)
        {
            this.context = context;
        }

        public ICollection<Order> GetPaginatedList(int pageNumber, int pageSize)
        {
            return context.Set<Order>().OrderBy(o => o.OrderDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public override Order? GetById(Guid id)
        {
            return context.Set<Order>()
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)!
                    .ThenInclude(oi => oi.Product)
                .SingleOrDefault(o => o.Id == id);
        }
    }
}
