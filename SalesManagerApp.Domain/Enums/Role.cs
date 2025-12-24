using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SalesManagerApp.Domain.Enums
{
    public enum Role
    {
        [Description("Administrador")]
        Admin = 1,
        [Description("Usuario")]
        User = 2
    }
}
