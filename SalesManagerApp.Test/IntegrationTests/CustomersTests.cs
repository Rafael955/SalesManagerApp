using Bogus;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using SalesManagerApp.Domain.Dtos.Requests;
using SalesManagerApp.Domain.Dtos.Responses;
using System.Net;
using System.Net.Http.Json;

namespace SalesManagerApp.Test.IntegrationTests
{
    public class CustomersTests
    {
        private readonly HttpClient _httpClient;

        public CustomersTests()
        {
            _httpClient = new WebApplicationFactory<Program>().CreateClient();
        }

        [Fact(DisplayName = "Deve criar um novo cliente com sucesso")]
        public void DeveCriarUmNovoClienteComSucesso()
        {
            Faker _faker = new Faker("pt_BR");

            var request = new CustomerRequestDto
            {
                Name = _faker.Person.FullName,
                Email = _faker.Person.Email,
                Phone = _faker.Phone.PhoneNumberFormat(0)
            };

            var response = _httpClient.PostAsJsonAsync("api/customers/register-customer", request).Result;

            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var content = response.Content.ReadAsStringAsync().Result;

            var objeto = JsonConvert.DeserializeObject<dynamic>(content);

            objeto.message.ToString().Should().Be("Cliente cadastrado com sucesso!");

            CustomerResponseDto customerResponse = JsonConvert.DeserializeObject<CustomerResponseDto>(objeto.data.ToString());

            customerResponse.Should().NotBeNull();
            customerResponse!.Id.Should().NotBeEmpty();
            customerResponse.Name.Should().Be(request.Name);
            customerResponse.Email.Should().Be(request.Email);
            customerResponse.Phone.Should().Be(request.Phone);
        }

        [Fact(DisplayName = "Deve retornar erro ao informar cliente com dados invalidos")]
        public void DeveRetornarErroAoInformarClienteComDadosInvalidos()
        {
            var request = new CustomerRequestDto
            {
                Name = "",
                Email = "emailinvalido",
                Phone = ""
            };

            var response = _httpClient.PostAsJsonAsync("api/customers/register-customer", request).Result;

            response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);

            var content = response.Content.ReadAsStringAsync().Result;

            List<ValidationErrorResponseDto> validationErrorResponseDto = JsonConvert.DeserializeObject<List<ValidationErrorResponseDto>>(content)!;

            validationErrorResponseDto.Should().NotBeNull();
            validationErrorResponseDto.Count.Should().BeGreaterThan(0);
        }

        [Fact(DisplayName = "Deve retornar erro ao tentar cadastrar novo usuário com email já cadastrado")]
        public void DeveRetornarErroAoTentarCadastrarNovoUsuarioComEmailJaCadastrado()
        {
            Faker _faker = new Faker("pt_BR");

            var request = new CustomerRequestDto
            {
                Name = _faker.Person.FullName,
                Email = _faker.Person.Email,
                Phone = _faker.Phone.PhoneNumberFormat(0)
            };

            var response = _httpClient.PostAsJsonAsync("api/customers/register-customer", request).Result;

            response.StatusCode.Should().Be(HttpStatusCode.Created);

            _faker = new Faker("pt_BR");

            var request2 = new CustomerRequestDto
            {
                Name = _faker.Person.FullName,
                Email = request.Email, // Mesmo email do primeiro cadastro
                Phone = _faker.Phone.PhoneNumberFormat(0)
            };

            var response2 = _httpClient.PostAsJsonAsync("api/customers/register-customer", request2).Result;

            response2.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var content = response2.Content.ReadAsStringAsync().Result;

            var errorMessageResponse = JsonConvert.DeserializeObject<ErrorResponseDto>(content);

            errorMessageResponse.Message.Should().Be("Já existe um cliente cadastrado com este e-mail.");
        }

        [Fact(DisplayName = "Deve atualizar um novo cliente com sucesso")]
        public void DeveAtualizarUmNovoClienteComSucesso()
        {
            Faker _faker = new Faker("pt_BR");

            var request = new CustomerRequestDto
            {
                Name = _faker.Person.FullName,
                Email = _faker.Person.Email,
                Phone = _faker.Phone.PhoneNumber()
            };

            var response = _httpClient.PostAsJsonAsync("api/customers/register-customer", request).Result;

            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var content = response.Content.ReadAsStringAsync().Result;

            var objeto = JsonConvert.DeserializeObject<dynamic>(content);

            CustomerResponseDto customerResponse = JsonConvert.DeserializeObject<CustomerResponseDto>(objeto.data.ToString());

            _faker = new Faker("pt_BR");

            request.Name = _faker.Person.FullName;
            request.Email = _faker.Person.Email;
            request.Phone = _faker.Phone.PhoneNumberFormat(0);

            var responseUpdate = _httpClient.PutAsJsonAsync($"api/customers/update-customer/{customerResponse.Id}", request).Result;

            responseUpdate.StatusCode.Should().Be(HttpStatusCode.OK);

            content = responseUpdate.Content.ReadAsStringAsync().Result;

            objeto = JsonConvert.DeserializeObject<dynamic>(content);

            objeto.message.ToString().Should().Be("Cliente alterado com sucesso!");

            customerResponse = JsonConvert.DeserializeObject<CustomerResponseDto>(objeto.data.ToString());

            customerResponse.Should().NotBeNull();
            customerResponse!.Id.Should().NotBeEmpty();
            customerResponse.Name.Should().Be(request.Name);
            customerResponse.Email.Should().Be(request.Email);
            customerResponse.Phone.Should().Be(request.Phone);
        }

        [Fact(DisplayName = "Deve retornar erro ao tentar atualizar usuário com email já cadastrado")]
        public void DeveRetornarErroAoTentarAtualizarUsuarioComEmailJaCadastrado()
        {
            Faker _faker = new Faker("pt_BR");

            var request = new CustomerRequestDto
            {
                Name = _faker.Person.FullName,
                Email = _faker.Person.Email,
                Phone = _faker.Phone.PhoneNumberFormat(0)
            };

            var response = _httpClient.PostAsJsonAsync("api/customers/register-customer", request).Result;

            response.StatusCode.Should().Be(HttpStatusCode.Created);

            _faker = new Faker("pt_BR");

            var request2 = new CustomerRequestDto
            {
                Name = _faker.Person.FullName,
                Email = _faker.Person.Email,
                Phone = _faker.Phone.PhoneNumberFormat(0)
            };

            var response2 = _httpClient.PostAsJsonAsync("api/customers/register-customer", request2).Result;

            response2.StatusCode.Should().Be(HttpStatusCode.Created);

            var content = response2.Content.ReadAsStringAsync().Result;

            var objeto = JsonConvert.DeserializeObject<dynamic>(content);

            CustomerResponseDto customerResponse2 = JsonConvert.DeserializeObject<CustomerResponseDto>(objeto.data.ToString());


            request2.Email = request.Email; // Mesmo email do primeiro cadastro


            var responseUpdate = _httpClient.PutAsJsonAsync($"api/customers/update-customer/{customerResponse2.Id}", request2).Result;

            responseUpdate.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            content = responseUpdate.Content.ReadAsStringAsync().Result;

            var errorMessageResponse = JsonConvert.DeserializeObject<ErrorResponseDto>(content);

            errorMessageResponse.Message.Should().Be("Já existe um cliente cadastrado com este e-mail.");
        }

        [Fact(DisplayName = "Deve retornar erro ao atualizar cliente com dados invalidos")]
        public void DeveRetornarErroAoAtualizarClienteComDadosInvalidos()
        {
            Faker _faker = new Faker("pt_BR");

            var request = new CustomerRequestDto
            {
                Name = _faker.Person.FullName,
                Email = _faker.Person.Email,
                Phone = _faker.Phone.PhoneNumberFormat(0)
            };

            var response = _httpClient.PostAsJsonAsync("api/customers/register-customer", request).Result;

            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var content = response.Content.ReadAsStringAsync().Result;

            var objeto = JsonConvert.DeserializeObject<dynamic>(content);

            CustomerResponseDto customerResponse = JsonConvert.DeserializeObject<CustomerResponseDto>(objeto.data.ToString());

            // Dados inválidos para atualização
            var invalidRequest = new CustomerRequestDto
            {
                Name = "",
                Email = "invalido",
                Phone = ""
            };

            var responseUpdate = _httpClient.PutAsJsonAsync($"api/customers/update-customer/{customerResponse.Id}", invalidRequest).Result;

            responseUpdate.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);

            content = responseUpdate.Content.ReadAsStringAsync().Result;

            List<ValidationErrorResponseDto> validationErrors = JsonConvert.DeserializeObject<List<ValidationErrorResponseDto>>(content)!;

            validationErrors.Should().NotBeNull();
            validationErrors.Count.Should().BeGreaterThan(0);
        }

        [Fact(DisplayName = "Deve retornar erro ao atualizar cliente inexistente")]
        public void DeveRetornarErroAoAtualizarClienteInexistente()
        {
            Faker _faker = new Faker("pt_BR");

            var invalidRequest = new CustomerRequestDto
            {
                Name = _faker.Person.FullName,
                Email = _faker.Person.Email,
                Phone = _faker.Phone.PhoneNumberFormat(0)
            };

            var randomId = Guid.NewGuid();

            var responseUpdate = _httpClient.PutAsJsonAsync($"api/customers/update-customer/{randomId}", invalidRequest).Result;

            responseUpdate.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var content = responseUpdate.Content.ReadAsStringAsync().Result;

            var errorResponse = JsonConvert.DeserializeObject<ErrorResponseDto>(content);

            errorResponse.Should().NotBeNull();
            errorResponse!.Message.Should().NotBeNullOrWhiteSpace();
        }

        [Fact(DisplayName = "Deve excluir um cliente com sucesso.")]
        public void DeveExcluirUmClienteComSucesso()
        {
            Faker _faker = new Faker("pt_BR");

            var request = new CustomerRequestDto
            {
                Name = _faker.Person.FullName,
                Email = _faker.Person.Email,
                Phone = _faker.Phone.PhoneNumberFormat(0)
            };

            var response = _httpClient.PostAsJsonAsync("api/customers/register-customer", request).Result;

            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var content = response.Content.ReadAsStringAsync().Result;

            var objeto = JsonConvert.DeserializeObject<dynamic>(content);

            CustomerResponseDto customerResponse = JsonConvert.DeserializeObject<CustomerResponseDto>(objeto.data.ToString());

            var responseDelete = _httpClient.DeleteAsync($"api/customers/delete-customer/{customerResponse.Id}").Result;

            responseDelete.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact(DisplayName = "Deve retornar erro ao excluir cliente inexistente")]
        public void DeveRetornarErroAoExcluirClienteInexistente()
        {
            var randomId = Guid.NewGuid();

            var response = _httpClient.DeleteAsync($"api/customers/delete-customer/{randomId}").Result;

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var content = response.Content.ReadAsStringAsync().Result;

            var errorResponse = JsonConvert.DeserializeObject<ErrorResponseDto>(content);

            errorResponse.Should().NotBeNull();
            errorResponse!.Message.Should().NotBeNullOrWhiteSpace();
        }

        [Fact(DisplayName = "Deve retornar um cliente com sucesso.")]
        public void DeveRetornarUmClienteComSucesso()
        {
            Faker _faker = new Faker("pt_BR");

            var request = new CustomerRequestDto
            {
                Name = _faker.Person.FullName,
                Email = _faker.Person.Email,
                Phone = _faker.Phone.PhoneNumberFormat(0)
            };

            var response = _httpClient.PostAsJsonAsync("api/customers/register-customer", request).Result;

            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var content = response.Content.ReadAsStringAsync().Result;

            var objeto = JsonConvert.DeserializeObject<dynamic>(content);

            CustomerResponseDto customerResponse = JsonConvert.DeserializeObject<CustomerResponseDto>(objeto.data.ToString());

            var responseGet = _httpClient.GetAsync($"api/customers/get-customer/{customerResponse.Id}").Result;

            responseGet.StatusCode.Should().Be(HttpStatusCode.OK);

            content = responseGet.Content.ReadAsStringAsync().Result;

            CustomerResponseDto customerResponseGet = JsonConvert.DeserializeObject<CustomerResponseDto>(content);

            customerResponseGet.Should().NotBeNull();
            customerResponseGet!.Id.Should().Be(customerResponse.Id);
            customerResponseGet.Name.Should().Be(request.Name);
            customerResponseGet.Email.Should().Be(request.Email);
            customerResponseGet.Phone.Should().Be(request.Phone);
            customerResponseGet.Orders.Should().NotBeNull();
        }

        [Fact(DisplayName = "Deve retornar erro ao obter cliente inexistente")]
        public void DeveRetornarErroAoObterClienteInexistente()
        {
            var randomId = Guid.NewGuid();

            var response = _httpClient.GetAsync($"api/customers/get-customer/{randomId}").Result;

            // O controller converte ApplicationException em 400
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var content = response.Content.ReadAsStringAsync().Result;

            var errorResponse = JsonConvert.DeserializeObject<ErrorResponseDto>(content);

            errorResponse.Should().NotBeNull();
            errorResponse!.Message.Should().NotBeNullOrWhiteSpace();
        }

        [Fact(DisplayName = "Deve retornar StatusCode 400 para id mal formado na rota")]
        public void DeveRetornarStatusCode400ParaIdMalFormadoNaRota()
        {
            var response = _httpClient.GetAsync($"api/customers/get-customer/abc").Result;

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact(DisplayName = "Deve retornar uma lista de clientes com sucesso.")]
        public void DeveRetornarUmaListaDeClientesComSucesso()
        {
            Faker _faker;

            foreach (var _ in Enumerable.Range(1, 5))
            {
                _faker = new Faker("pt_BR");

                var request = new CustomerRequestDto
                {
                    Name = _faker.Person.FullName,
                    Email = _faker.Person.Email,
                    Phone = _faker.Phone.PhoneNumberFormat(0)
                };
                
                var response = _httpClient.PostAsJsonAsync("api/customers/register-customer", request).Result;

                response.StatusCode.Should().Be(HttpStatusCode.Created);
            }

            var responseList = _httpClient.GetAsync($"api/customers/list-customers").Result;

            responseList.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = responseList.Content.ReadAsStringAsync().Result;

            List<CustomerResponseDto> customerResponseList = JsonConvert.DeserializeObject<List<CustomerResponseDto>>(content);

            customerResponseList.Should().NotBeNull();
            customerResponseList.Count.Should().BeGreaterThanOrEqualTo(5);
            customerResponseList.All(c => c.Id != Guid.Empty).Should().BeTrue();
            customerResponseList.All(c => !string.IsNullOrEmpty(c.Name)).Should().BeTrue();
            customerResponseList.All(c => !string.IsNullOrEmpty(c.Email)).Should().BeTrue();
            customerResponseList.All(c => !string.IsNullOrEmpty(c.Phone)).Should().BeTrue();
            customerResponseList.All(c => c.Orders == null).Should().BeTrue();
        }
    }
}
