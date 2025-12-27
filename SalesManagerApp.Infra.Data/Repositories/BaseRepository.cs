using SalesManagerApp.Domain.Entities;
using SalesManagerApp.Domain.Interfaces.Repositories;
using SalesManagerApp.Infra.Data.Contexts;

namespace SalesManagerApp.Infra.Data.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        public void Add(T entity)
        {
            using (var context = new DataContext())
            {
                context.Add(entity);
                context.SaveChanges();
            }
        }

        public void Update(T entity)
        {
            using (var context = new DataContext())
            {
                context.Update(entity);
                context.SaveChanges();
            }
        }

        public void Delete(T entity)
        {
            using (var context = new DataContext())
            {
                entity.IsActive = false;
                context.Update(entity); //SOFT DELETE
                context.SaveChanges();
            }
        }

        public T? GetById(Guid id)
        {
            using (var context = new DataContext())
            {
                return context.Set<T>().SingleOrDefault(x => x.Id == id);
            }
        }

        public List<T>? GetAll()
        {
            using (var context = new DataContext())
            {
                return context.Set<T>().ToList();
            }
        }
    }
}
