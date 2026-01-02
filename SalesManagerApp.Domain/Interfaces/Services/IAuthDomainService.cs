using SalesManagerApp.Domain.Dtos.Requests;
using SalesManagerApp.Domain.Dtos.Responses;

namespace SalesManagerApp.Domain.Interfaces.Services
{
    public interface IAuthDomainService
    {
        UserLoginResponseDto AutenticarUsuario(UserLoginRequestDto request);
    }
}
