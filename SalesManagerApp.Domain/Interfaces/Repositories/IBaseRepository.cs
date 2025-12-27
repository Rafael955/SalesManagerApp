namespace SalesManagerApp.Domain.Interfaces.Repositories
{
    public interface IBaseRepository<T> where T : class
    {
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        T? GetById(Guid id);
        List<T>? GetAll();
    }
}
