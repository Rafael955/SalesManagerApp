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
    public class CustomerTests
    {
        private readonly HttpClient _httpClient;

        public CustomerTests()
        {
            _httpClient = new WebApplicationFactory<Program>().CreateClient();
        }

        [Fact(DisplayName = "Deve criar um novo cliente com sucesso.")]
        public void DeveCriarUmNovoClienteComSucesso() 
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

            customerResponse.Should().NotBeNull();
            customerResponse!.Id.Should().NotBeEmpty();
            customerResponse.Name.Should().Be(request.Name);
            customerResponse.Email.Should().Be(request.Email);
            customerResponse.Phone.Should().Be(request.Phone);
        }

        [Fact(DisplayName = "Deve retornar erro ao informar dados invalidos")]
        public void DeveRetornarErroAoInformarDadosInvalidos()
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

        [Fact(DisplayName = "Deve atualizar um novo cliente com sucesso.")]
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
    }
}
