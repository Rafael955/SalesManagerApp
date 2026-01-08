using Microsoft.EntityFrameworkCore;
using SalesManagerApp.Domain.Entities;
using SalesManagerApp.Domain.Interfaces.Repositories;
using SalesManagerApp.Infra.Data.Contexts;

namespace SalesManagerApp.Infra.Data.Repositories
{
    public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
    {
        private readonly DataContext context;

        public CustomerRepository(DataContext context) : base(context)
        {
            this.context = context;
        }

        public Customer? GetCustomerByEmail(string email)
        {
            return context.Set<Customer>()
                .SingleOrDefault(c => c.Email == email);
        }

        public override Customer? GetById(Guid id)
        {
            return context.Set<Customer>()
                .Include(c => c.Orders)
                .SingleOrDefault(c => c.Id == id);
        }
    }
}
