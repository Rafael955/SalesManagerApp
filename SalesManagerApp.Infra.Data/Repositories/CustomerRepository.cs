using Microsoft.EntityFrameworkCore;
using SalesManagerApp.Domain.Entities;
using SalesManagerApp.Domain.Interfaces.Repositories;
using SalesManagerApp.Infra.Data.Contexts;

namespace SalesManagerApp.Infra.Data.Repositories
{
    public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
    {
        public Customer? GetCustomerByEmail(string email)
        {
            using (var context = new DataContext())
            {
                return context.Set<Customer>()
                    .SingleOrDefault(c => c.Email == email);
            }
        }

        public override Customer? GetById(Guid id)
        {
            using (var context = new DataContext())
            {
                return context.Set<Customer>()
                    .Include(c => c.Orders)
                    .SingleOrDefault(c => c.Id == id);
            }
        }
    }
}
