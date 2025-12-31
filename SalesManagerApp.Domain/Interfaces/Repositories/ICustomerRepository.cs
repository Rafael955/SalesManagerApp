using SalesManagerApp.Domain.Entities;

namespace SalesManagerApp.Domain.Interfaces.Repositories
{
    public interface ICustomerRepository : IBaseRepository<Customer>
    {
        Customer? GetCustomerByEmail(string email);
    }
}
