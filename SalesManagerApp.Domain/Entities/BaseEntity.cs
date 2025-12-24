namespace SalesManagerApp.Domain.Entities
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
