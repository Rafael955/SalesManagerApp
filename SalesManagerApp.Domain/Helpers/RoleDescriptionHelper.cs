using SalesManagerApp.Domain.Enums;
using System.ComponentModel;
using System.Reflection;

namespace SalesManagerApp.Domain.Helpers
{
    public static class RoleDescriptionHelper
    {
        public static string GetDescription(this Role perfil)
        {
            //Obtém o campo do enum correspondente ao valor atual.
            var field = perfil.GetType().GetField(perfil.ToString());

            //Busca o atributo [Description] aplicado ao campo.
            var attr = field?.GetCustomAttribute<DescriptionAttribute>();

            //Se o atributo existir, retorna sua descrição; caso contrário, retorna o nome do enum.
            return attr?.Description ?? perfil.ToString();
        }
    }
}
