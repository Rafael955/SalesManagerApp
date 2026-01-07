using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SalesManagerApp.Controllers;
using SalesManagerApp.Domain.Dtos.Requests;
using SalesManagerApp.Domain.Dtos.Responses;
using SalesManagerApp.Domain.Interfaces.Services;
using System.Security.Authentication;

namespace SalesManagerApp.Test.UnitTests
{
    public class AuthControllerUnitTests
    {
        [Fact(DisplayName = "Login deve retornar 500 quando o service lança Exception")]
        public void Login_DeveRetornarStatus500_QuandoServicoLancarException()
        {
            var mockService = new Mock<IAuthDomainService>();

            mockService
                .Setup(s => s.AutenticarUsuario(It.IsAny<UserLoginRequestDto>()))
                .Throws(new Exception("Erro de servidor"));

            var controller = new AuthController(mockService.Object);

            var request = new UserLoginRequestDto
            {
                Email = "admin@admin.com",
                Password = "Admin@12345"
            };

            var result = controller.Login(request);

            var objectResult = result as ObjectResult;

            objectResult.Should().NotBeNull();
            objectResult.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);

            var error = objectResult.Value as ErrorResponseDto;

            error.Should().NotBeNull();
            error!.Message.Should().Be("Erro de servidor");
        }

        [Fact(DisplayName = "Login deve retornar 401 quando o service lança AuthenticationException")]
        public void Login_DeveRetornarStatus401_QuandoServicoLancarAuthenticationException()
        {
            var mockService = new Mock<IAuthDomainService>();

            mockService
                .Setup(s => s.AutenticarUsuario(It.IsAny<UserLoginRequestDto>()))
                .Throws(new AuthenticationException("Acesso negado. Credenciais inválidas."));

            var controller = new AuthController(mockService.Object);

            var request = new UserLoginRequestDto { Email = "x@y.com", Password = "Senha@123" };

            var result = controller.Login(request);

            var objectResult = result as ObjectResult;

            objectResult.Should().NotBeNull();
            objectResult!.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);

            var error = objectResult.Value as ErrorResponseDto;

            error.Should().NotBeNull();
            error!.Message.Should().Be("Acesso negado. Credenciais inválidas.");
        }

        [Fact(DisplayName = "Login deve retornar 422 quando o service lança ValidationException")]
        public void Login_RetornaStatus422_QuandoServicoLancarValidationException()
        {
            var failures = new List<ValidationFailure>
            {
                new ValidationFailure("Email", "Email inválido")
            };

            var validationException = new ValidationException(failures);

            var mockService = new Mock<IAuthDomainService>();

            mockService
                .Setup(s => s.AutenticarUsuario(It.IsAny<UserLoginRequestDto>()))
                .Throws(validationException);

            var controller = new AuthController(mockService.Object);

            var request = new UserLoginRequestDto { Email = "emailinvalido", Password = "fraco" };

            var result = controller.Login(request);

            var objectResult = result as ObjectResult;

            objectResult.Should().NotBeNull();
            objectResult!.StatusCode.Should().Be(StatusCodes.Status422UnprocessableEntity);

            var errors = objectResult.Value as IEnumerable<ValidationErrorResponseDto>;

            errors.Should().NotBeNull();
            errors!.Should().ContainSingle(e => e.PropertyName == "Email" && e.ErrorMessage == "Email inválido");
        }

        [Fact(DisplayName = "Login deve retornar 200 quando o service realiza um login com sucesso")]
        public void Login_RetornaStatus200_QuandoServicoRetornaSucesso()
        {
            var mockService = new Mock<IAuthDomainService>();

            var userLoginResponseDto = new UserLoginResponseDto
            {
                Id = Guid.NewGuid(),
                Name = "Admin User",
                Email = "admin@admin.com",
                Role = "Administrator",
                AccessToken = "token_sessao"
            };

            mockService
                .Setup(s => s.AutenticarUsuario(It.IsAny<UserLoginRequestDto>()))
                .Returns(userLoginResponseDto);

            var controller = new AuthController(mockService.Object);

            var request = new UserLoginRequestDto
            {
                Email = "admin@admin.com",
                Password = "Admin@12345"
            };

            var result = controller.Login(request);

            var objectResult = result as ObjectResult;

            objectResult.Should().NotBeNull();
            objectResult!.StatusCode.Should().Be(StatusCodes.Status200OK);

            var userLoginResponseValue = objectResult.Value! as UserLoginResponseDto;

            userLoginResponseValue.Should().NotBeNull();
            userLoginResponseValue!.Id.Should().Be(userLoginResponseDto.Id);
            userLoginResponseValue.Name.Should().Be("Admin User");
            userLoginResponseValue.Email.Should().Be("admin@admin.com");
            userLoginResponseValue.Role.Should().Be("Administrator");
            userLoginResponseValue.AccessToken.Should().Be("token_sessao");
        }
    }
}