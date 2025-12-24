using SalesManagerApp.Domain.Dtos.Requests;
using SalesManagerApp.Domain.Dtos.Responses;
using SalesManagerApp.Domain.Entities;

namespace SalesManagerApp.Domain.Interfaces.Repositories
{
    public interface IAuthRepository : IBaseRepository<Order>
    {
        UserLoginResponseDto AuthUser(UserLoginRequestDto request);
    }
}
