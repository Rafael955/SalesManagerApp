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
    public class ProductsIntegrationTests
    {
        private readonly HttpClient _httpClient;

        public ProductsIntegrationTests()
        {
            _httpClient = new WebApplicationFactory<Program>().CreateClient();
        }

        [Fact(DisplayName = "Deve criar um novo produto com sucesso")]
        public void DeveCriarUmNovoProdutoComSucesso()
        {
            var faker = new Faker("pt_BR");

            var request = new ProductRequestDto
            {
                Name = faker.Commerce.ProductName(),
                Price = Convert.ToDecimal(faker.Commerce.Price()),
                Quantity = faker.Random.Int(1, 100)
            };

            var response = _httpClient.PostAsJsonAsync("api/products", request).Result;

            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var content = response.Content.ReadAsStringAsync().Result;
            var objeto = JsonConvert.DeserializeObject<dynamic>(content)!;

            string message = objeto.message.ToString();

            message.Should().Be("Produto criado com sucesso");

            ProductResponseDto productResponse = JsonConvert.DeserializeObject<ProductResponseDto>(objeto.data.ToString());

            productResponse.Should().NotBeNull();
            productResponse!.Id.Should().NotBeEmpty();
            productResponse.Name.Should().Be(request.Name);
            productResponse.Price.Should().Be(request.Price);
            productResponse.Quantity.Should().Be(request.Quantity);
        }

        [Fact(DisplayName = "Deve retornar erro ao informar produto com dados invalidos")]
        public void DeveRetornarErroAoInformarProdutoComDadosInvalidos()
        {
            var invalidRequest = new ProductRequestDto
            {
                Name = "",
                Price = -10,
                Quantity = -1
            };

            var response = _httpClient.PostAsJsonAsync("api/products", invalidRequest).Result;

            response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);

            var content = response.Content.ReadAsStringAsync().Result;
            List<ValidationErrorResponseDto> validationErrors = JsonConvert.DeserializeObject<List<ValidationErrorResponseDto>>(content)!;

            validationErrors.Should().NotBeNull();
            validationErrors.Count.Should().BeGreaterThan(0);
        }

        [Fact(DisplayName = "Deve atualizar produto com sucesso")]
        public void DeveAtualizarProdutoComSucesso()
        {
            var faker = new Faker("pt_BR");

            var createRequest = new ProductRequestDto
            {
                Name = faker.Commerce.ProductName(),
                Price = Convert.ToDecimal(faker.Commerce.Price()),
                Quantity = 10
            };

            var responseCreated = _httpClient.PostAsJsonAsync("api/products", createRequest).Result;

            responseCreated.StatusCode.Should().Be(HttpStatusCode.Created);

            var contentCreated = responseCreated.Content.ReadAsStringAsync().Result;

            var objetoCreated = JsonConvert.DeserializeObject<dynamic>(contentCreated);

            ProductResponseDto productCreatedResponseDto = JsonConvert.DeserializeObject<ProductResponseDto>(objetoCreated.data.ToString());

            var updateRequest = new ProductRequestDto
            {
                Name = faker.Commerce.ProductName(),
                Price = Convert.ToDecimal(faker.Commerce.Price()),
                Quantity = 20
            };

            var responseUpdate = _httpClient.PutAsJsonAsync($"api/products/{productCreatedResponseDto.Id}", updateRequest).Result;

            responseUpdate.StatusCode.Should().Be(HttpStatusCode.OK);

            var contentUpdate = responseUpdate.Content.ReadAsStringAsync().Result;

            var objetoUpdate = JsonConvert.DeserializeObject<dynamic>(contentUpdate)!;

            string message = objetoUpdate.message.ToString();

            message.Should().Be("Produto alterado com sucesso");

            ProductResponseDto productUpdatedResponseDto = JsonConvert.DeserializeObject<ProductResponseDto>(objetoUpdate.data?.ToString());

            productUpdatedResponseDto.Should().NotBeNull();
            productUpdatedResponseDto!.Id.Should().Be(productCreatedResponseDto.Id);
            productUpdatedResponseDto.Name.Should().Be(updateRequest.Name);
            productUpdatedResponseDto.Price.Should().Be(updateRequest.Price);
            productUpdatedResponseDto.Quantity.Should().Be(updateRequest.Quantity);
        }

        [Fact(DisplayName = "Deve retornar erro ao atualizar produto com dados invalidos")]
        public void DeveRetornarErroAoAtualizarProdutoComDadosInvalidos()
        {
            var faker = new Faker("pt_BR");

            var createRequest = new ProductRequestDto
            {
                Name = faker.Commerce.ProductName(),
                Price = Convert.ToDecimal(faker.Commerce.Price()),
                Quantity = 10
            };

            var responseCreated = _httpClient.PostAsJsonAsync("api/products", createRequest).Result;
            responseCreated.StatusCode.Should().Be(HttpStatusCode.Created);

            var contentCreated = responseCreated.Content.ReadAsStringAsync().Result;
            
            var objetoCreated = JsonConvert.DeserializeObject<dynamic>(contentCreated);

            ProductResponseDto created = JsonConvert.DeserializeObject<ProductResponseDto>(objetoCreated.data.ToString());

            var invalidUpdate = new ProductRequestDto
            {
                Name = "",
                Price = -1,
                Quantity = -5
            };

            var responseUpdate = _httpClient.PutAsJsonAsync($"api/products/{created.Id}", invalidUpdate).Result;
            
            responseUpdate.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);

            var content = responseUpdate.Content.ReadAsStringAsync().Result;

            List<ValidationErrorResponseDto> validationErrors = JsonConvert.DeserializeObject<List<ValidationErrorResponseDto>>(content)!;

            validationErrors.Should().NotBeNull();
            validationErrors.Count.Should().BeGreaterThan(0);
        }

        [Fact(DisplayName = "Deve retornar erro ao atualizar produto inexistente")]
        public void DeveRetornarErroAoAtualizarProdutoInexistente()
        {
            var faker = new Faker("pt_BR");

            var updateRequest = new ProductRequestDto
            {
                Name = faker.Commerce.ProductName(),
                Price = Convert.ToDecimal(faker.Commerce.Price()),
                Quantity = 5
            };

            var randomId = Guid.NewGuid();

            var response = _httpClient.PutAsJsonAsync($"api/products/{randomId}", updateRequest).Result;

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var content = response.Content.ReadAsStringAsync().Result;
            var errorResponse = JsonConvert.DeserializeObject<ErrorResponseDto>(content);

            errorResponse.Should().NotBeNull();
            errorResponse!.Message.Should().NotBeNullOrWhiteSpace();
            errorResponse.Message.Should().Be("O produto com este Id não existe!");
        }

        [Fact(DisplayName = "Deve excluir produto com sucesso")]
        public void DeveExcluirProdutoComSucesso()
        {
            var faker = new Faker("pt_BR");

            var createRequest = new ProductRequestDto
            {
                Name = faker.Commerce.ProductName(),
                Price = Convert.ToDecimal(faker.Commerce.Price()),
                Quantity = 10
            };

            var responseCreated = _httpClient.PostAsJsonAsync("api/products", createRequest).Result;
            
            responseCreated.StatusCode.Should().Be(HttpStatusCode.Created);

            var contentCreated = responseCreated.Content.ReadAsStringAsync().Result;
            
            var objetoCreated = JsonConvert.DeserializeObject<dynamic>(contentCreated);

            ProductResponseDto productCreatedResponseDto = JsonConvert.DeserializeObject<ProductResponseDto>(objetoCreated.data.ToString());

            var responseDeleted = _httpClient.DeleteAsync($"api/products/{productCreatedResponseDto.Id}").Result;

            responseDeleted.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact(DisplayName = "Deve retornar erro ao excluir produto inexistente")]
        public void DeveRetornarErroAoExcluirProdutoInexistente()
        {
            var randomId = Guid.NewGuid();

            var response = _httpClient.DeleteAsync($"api/products/{randomId}").Result;
            
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var content = response.Content.ReadAsStringAsync().Result;
            
            var errorResponse = JsonConvert.DeserializeObject<ErrorResponseDto>(content);

            errorResponse.Should().NotBeNull();
            errorResponse!.Message.Should().NotBeNullOrWhiteSpace();
            errorResponse.Message.Should().Be("O produto com este Id não existe!");
        }

        [Fact(DisplayName = "Deve retornar um produto com sucesso")]
        public void DeveRetornarUmProdutoComSucesso()
        {
            var faker = new Faker("pt_BR");

            var createRequest = new ProductRequestDto
            {
                Name = faker.Commerce.ProductName(),
                Price = Convert.ToDecimal(faker.Commerce.Price()),
                Quantity = 10
            };

            var responseCreated = _httpClient.PostAsJsonAsync("api/products", createRequest).Result;

            responseCreated.StatusCode.Should().Be(HttpStatusCode.Created);

            var contentCreated = responseCreated.Content.ReadAsStringAsync().Result;

            var objetoCreated = JsonConvert.DeserializeObject<dynamic>(contentCreated);

            ProductResponseDto created = JsonConvert.DeserializeObject<ProductResponseDto>(objetoCreated.data.ToString());

            var responseGet = _httpClient.GetAsync($"api/products/{created.Id}").Result;

            responseGet.StatusCode.Should().Be(HttpStatusCode.OK);

            var contentGet = responseGet.Content.ReadAsStringAsync().Result;

            ProductResponseDto productResponseDto = JsonConvert.DeserializeObject<ProductResponseDto>(contentGet)!;

            productResponseDto.Should().NotBeNull();
            productResponseDto!.Id.Should().Be(created.Id);
            productResponseDto.Name.Should().Be(createRequest.Name);
            productResponseDto.Price.Should().Be(createRequest.Price);
            productResponseDto.Quantity.Should().Be(createRequest.Quantity);
        }

        [Fact(DisplayName = "Deve retornar erro ao obter produto inexistente")]
        public void DeveRetornarErroAoObterProdutoInexistente()
        {
            var randomId = Guid.NewGuid();

            var response = _httpClient.GetAsync($"api/products/{randomId}").Result;

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var content = response.Content.ReadAsStringAsync().Result;

            var errorResponse = JsonConvert.DeserializeObject<ErrorResponseDto>(content);

            errorResponse.Should().NotBeNull();
            errorResponse!.Message.Should().NotBeNullOrWhiteSpace();
        }

        [Fact(DisplayName = "Deve retornar StatusCode 400 para id mal formado na rota")]
        public void DeveRetornarStatusCode400ParaIdMalFormadoNaRota()
        {
            var response = _httpClient.GetAsync($"api/products/abc").Result;

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact(DisplayName = "Deve retornar uma lista de produtos com sucesso")]
        public void DeveRetornarUmaListaDeProdutosComSucesso()
        {
            var faker = new Faker("pt_BR");

            foreach (var _ in Enumerable.Range(1, 5))
            {
                var request = new ProductRequestDto
                {
                    Name = faker.Commerce.ProductName(),
                    Price = Convert.ToDecimal(faker.Commerce.Price()),
                    Quantity = faker.Random.Int(1, 100)
                };

                var response = _httpClient.PostAsJsonAsync("api/products", request).Result;

                response.StatusCode.Should().Be(HttpStatusCode.Created);
            }

            var responseList = _httpClient.GetAsync("api/products").Result;

            responseList.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = responseList.Content.ReadAsStringAsync().Result;

            List<ProductResponseDto> products = JsonConvert.DeserializeObject<List<ProductResponseDto>>(content)!;

            products.Should().NotBeNull();
            products.Count.Should().BeGreaterThanOrEqualTo(5);
            products.All(p => p.Id != Guid.Empty).Should().BeTrue();
            products.All(p => !string.IsNullOrEmpty(p.Name)).Should().BeTrue();
        }
    }
}