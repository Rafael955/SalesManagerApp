using System.ComponentModel;

namespace SalesManagerApp.Domain.Enums
{
    public enum OrderStatus
    {
        [Description("Pendente")]
        Pending,
        [Description("Aprovado")]
        Approved,
        [Description("Em progresso")]
        InProgress,
        [Description("Finalizado")]
        Completed,
        [Description("Cancelado")]
        Cancelled    
    }
}
