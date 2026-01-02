using Bogus;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using SalesManagerApp.Domain.Dtos.Requests;
using SalesManagerApp.Domain.Dtos.Responses;
using System.Net;
using System.Net.Http.Json;

namespace SalesManagerApp.Test.Tests
{
    public class AuthTests
    {
        private readonly HttpClient _httpClient;
        private readonly Faker _faker;

        public AuthTests()
        {
            _httpClient = new WebApplicationFactory<Program>().CreateClient();
            _faker = new Faker("pt_BR");
        }

        [Fact(DisplayName = "Deve realizar login do usuário com sucesso.")]
        public void DeveRealizarLoginDoUsuarioComSucesso()
        {
            var request = new UserLoginRequestDto
            {
                Email = "admin@admin.com",
                Password = "Admin@12345"
            };

            //Act : Enviando a requisição para a API
            var response = _httpClient.PostAsJsonAsync("/api/auth/login", request).Result;

            //Assert : Verificando se o resultado é sucesso HTTP 200 OK
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = response.Content.ReadAsStringAsync().Result;

            var userLoginResponse = JsonConvert.DeserializeObject<UserLoginResponseDto>(content);

            userLoginResponse.Should().NotBeNull();
            userLoginResponse!.Id.Should().NotBe(Guid.Empty);
            userLoginResponse.Name.Should().Be("Admin");
            userLoginResponse.Email.Should().Be("admin@admin.com");
            userLoginResponse.Role.Should().Be("Administrador");
            userLoginResponse.AccessToken.Should().NotBeNullOrEmpty();
        }

        [Fact(DisplayName = "Deve retornar acesso negado para usuário invalido.")]
        public void DeveRetornarAcessoNegadoParaUsuarioInvalido()
        {
            var request = new UserLoginRequestDto
            {
                Email = _faker.Person.Email,
                Password = "SenhaForte@123"
            };

            //Act : Enviando a requisição para a API
            var response = _httpClient.PostAsJsonAsync("/api/auth/login", request).Result;

            //Assert : Verificando se o resultado é HTTP 401 Unauthorized
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

            var content = response.Content.ReadAsStringAsync()?.Result;

            var errorMessageResponse = JsonConvert.DeserializeObject<ErrorResponseDto>(content);

            errorMessageResponse.Message.Should().Be("Acesso negado. Credenciais inválidas.");
        }
    }
}
