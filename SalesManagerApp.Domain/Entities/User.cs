using SalesManagerApp.Domain.Enums;
using System.ComponentModel;

namespace SalesManagerApp.Domain.Entities
{
    public class User : BaseEntity
    {
        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? Password { get; set; }

        public Role Role { get; set; }
    }
}
