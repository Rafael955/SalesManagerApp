using Microsoft.EntityFrameworkCore;
using SalesManagerApp.Infra.Data.Contexts;

namespace SalesManagerApp.Configurations
{
    public static class DataContextConfiguration
    {
        public static void AddDataContextConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<DataContext>(options => options.UseSqlServer(connectionString));
        }
    }
}
