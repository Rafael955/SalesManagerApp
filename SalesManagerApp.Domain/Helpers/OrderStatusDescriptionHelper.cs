using SalesManagerApp.Domain.Enums;
using System.ComponentModel;
using System.Reflection;

namespace SalesManagerApp.Domain.Helpers
{
    public static class OrderStatusDescriptionHelper
    {
        public static string GetDescription(this OrderStatus statusPedido)
        {
            //Obtém o campo do enum correspondente ao valor atual.
            var field = statusPedido.GetType().GetField(statusPedido.ToString());

            //Busca o atributo [Description] aplicado ao campo.
            var attr = field?.GetCustomAttribute<DescriptionAttribute>();

            //Se o atributo existir, retorna sua descrição; caso contrário, retorna o nome do enum.
            return attr?.Description ?? statusPedido.ToString();
        }
    }
}
