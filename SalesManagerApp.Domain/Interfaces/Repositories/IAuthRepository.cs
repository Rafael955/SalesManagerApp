using SalesManagerApp.Domain.Entities;

namespace SalesManagerApp.Domain.Interfaces.Repositories
{
    public interface IAuthRepository : IBaseRepository<User>
    {
        User? AuthUser(string email, string password);
    }
}
