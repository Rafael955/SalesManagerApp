using SalesManagerApp.Domain.Entities;
using SalesManagerApp.Domain.Interfaces.Repositories;
using SalesManagerApp.Infra.Data.Contexts;

namespace SalesManagerApp.Infra.Data.Repositories
{
    public class AuthRepository : BaseRepository<User>, IAuthRepository
    {
        public User? AuthUser(string email, string password)
        {
            using (var context = new DataContext())
            {
                return context.Set<User>()
                    .FirstOrDefault(u => u.Email == email && u.Password == password);
            }
        }
    }
}
