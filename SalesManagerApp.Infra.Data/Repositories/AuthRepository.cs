using SalesManagerApp.Domain.Entities;
using SalesManagerApp.Domain.Interfaces.Repositories;
using SalesManagerApp.Infra.Data.Contexts;

namespace SalesManagerApp.Infra.Data.Repositories
{
    public class AuthRepository : BaseRepository<User>, IAuthRepository
    {
        private readonly DataContext context;

        public AuthRepository(DataContext context) : base(context)
        {
            this.context = context;
        }

        public User? AuthUser(string email, string password)
        {
            return context.Set<User>()
                .FirstOrDefault(u => u.Email == email && u.Password == password);
        }
    }
}
