using SalesManagerApp.Domain.Entities;
using SalesManagerApp.Domain.Interfaces.Repositories;

namespace SalesManagerApp.Infra.Data.Repositories
{
    public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
    {

    }
}
