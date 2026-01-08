using SalesManagerApp.Domain.Entities;
using SalesManagerApp.Domain.Interfaces.Repositories;
using SalesManagerApp.Infra.Data.Contexts;

namespace SalesManagerApp.Infra.Data.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        private readonly DataContext context;

        public BaseRepository(DataContext context) 
        {
            this.context = context;
        }

        public void Add(T entity)
        {
            context.Add(entity);
            context.SaveChanges();
        }

        public void Update(T entity)
        {
            context.Update(entity);
            context.SaveChanges();
        }

        public void Delete(T entity)
        {
            entity.IsActive = false;
            context.Update(entity); //SOFT DELETE
            context.SaveChanges();
        }

        public virtual T? GetById(Guid id)
        {
            return context.Set<T>().SingleOrDefault(x => x.Id == id);
        }

        public virtual List<T> GetAll()
        {
            return context.Set<T>().ToList();
        }
    }
}
