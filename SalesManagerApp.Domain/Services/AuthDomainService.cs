using FluentValidation;
using SalesManagerApp.Domain.Dtos.Requests;
using SalesManagerApp.Domain.Dtos.Responses;
using SalesManagerApp.Domain.Helpers;
using SalesManagerApp.Domain.Interfaces.Repositories;
using SalesManagerApp.Domain.Interfaces.Services;
using SalesManagerApp.Domain.Validations;
using System.Security.Authentication;

namespace SalesManagerApp.Domain.Services
{
    public class AuthDomainService(IAuthRepository authRepository) : IAuthDomainService
    {
        public UserLoginResponseDto AutenticarUsuario(UserLoginRequestDto request)
        {
            var validation = new UserLoginValidator().Validate(request);

            if(!validation.IsValid)
                throw new ValidationException(validation.Errors);

            var userData = authRepository.AuthUser(request.Email!, CryptoHelper.GetSHA256(request.Password!));

            if(userData == null)
                throw new AuthenticationException("Acesso negado. Credenciais inválidas.");

            return new UserLoginResponseDto
            {
                Id = userData.Id,
                Name = userData.Name,
                Email = userData.Email,
                Role = userData.Role.GetDescription(),
                AccessToken = JwtTokenHelper.GenerateToken(userData.Email!, userData.Role.GetDescription())
            };
        }
    }
}
