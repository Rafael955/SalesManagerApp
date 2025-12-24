namespace SalesManagerApp.Domain.Interfaces.Repositories
{
    public interface IBaseRepository<T> where T : class
    {
        T Add(T entity);

        T Update(T entity);

        T Delete(T entity);

        T GetById(Guid id);

        List<T> GetAll();
    }
}
