using Bogus;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SalesManagerApp.Domain.Dtos.Requests;
using SalesManagerApp.Domain.Dtos.Responses;
using SalesManagerApp.Domain.Enums;
using System;
using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace SalesManagerApp.Test.IntegrationTests
{
    public class OrdersIntegrationTests
    {
        private readonly HttpClient _httpClient;

        public OrdersIntegrationTests()
        {
            _httpClient = new WebApplicationFactory<Program>().CreateClient();
        }

        [Fact(DisplayName = "Deve criar um novo pedido com sucesso")]
        public void DeveCriarUmNovoPedidoComSucesso()
        {
            Faker _faker = new Faker("pt_BR");

            // Criar cliente
            var customerRequest = new CustomerRequestDto
            {
                Name = _faker.Person.FullName,
                Email = _faker.Person.Email,
                Phone = _faker.Phone.PhoneNumberFormat(0)
            };

            var responseCustomer = _httpClient.PostAsJsonAsync("api/customers", customerRequest).Result;
            responseCustomer.StatusCode.Should().Be(HttpStatusCode.Created);

            var contentCustomer = responseCustomer.Content.ReadAsStringAsync().Result;

            var objetoCustomer = JsonConvert.DeserializeObject<dynamic>(contentCustomer);

            CustomerResponseDto customerResponse = JsonConvert.DeserializeObject<CustomerResponseDto>(objetoCustomer.data.ToString());

            // Criar produto
            var productRequest = new ProductRequestDto
            {
                Name = _faker.Commerce.ProductName(),
                Price = Convert.ToDecimal(_faker.Commerce.Price()),
                Quantity = 10
            };

            var responseProduct = _httpClient.PostAsJsonAsync("api/products", productRequest).Result;

            responseProduct.StatusCode.Should().Be(HttpStatusCode.Created);

            var contentProduct = responseProduct.Content.ReadAsStringAsync().Result;

            var objetoProduct = JsonConvert.DeserializeObject<dynamic>(contentProduct);

            ProductResponseDto productResponse = JsonConvert.DeserializeObject<ProductResponseDto>(objetoProduct.data.ToString());

            // Criar pedido
            var orderRequest = new CreateOrderRequestDto
            {
                CustomerId = customerResponse.Id,
                OrderItems = new List<CreateOrderItemRequestDto>
                {
                    new CreateOrderItemRequestDto
                    {
                        ProductId = productResponse.Id,
                        Quantity = 2
                    }
                }
            };

            var responseOrder = _httpClient.PostAsJsonAsync("api/orders", orderRequest).Result;

            responseOrder.StatusCode.Should().Be(HttpStatusCode.Created);

            var contentOrder = responseOrder.Content.ReadAsStringAsync().Result;

            var objetoOrder = JsonConvert.DeserializeObject<dynamic>(contentOrder);

            string message = objetoOrder.message?.ToString();

            message.Should().Be("Pedido criado com sucesso");

            OrderResponseDto orderResponse = JsonConvert.DeserializeObject<OrderResponseDto>(objetoOrder.data.ToString());

            orderResponse.Should().NotBeNull();
            orderResponse!.Id.Should().NotBeEmpty();
            orderResponse.CustomerId.Should().Be(customerResponse.Id);
            orderResponse.OrderItems.Should().NotBeNull();
            orderResponse.OrderItems!.Count.Should().BeGreaterThan(0);
            orderResponse.OrderItems.All(oi => !string.IsNullOrEmpty(oi.ProductName)).Should().BeTrue();
        }

        [Fact(DisplayName = "Deve retornar erro ao informar pedido com dados invalidos")]
        public void DeveRetornarErroAoInformarPedidoComDadosInvalidos()
        {
            var invalidRequest = new CreateOrderRequestDto
            {
                CustomerId = Guid.Empty,
                OrderItems = new List<CreateOrderItemRequestDto>() // vazio
            };

            var response = _httpClient.PostAsJsonAsync("api/orders", invalidRequest).Result;

            response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);

            var content = response.Content.ReadAsStringAsync().Result;

            List<ValidationErrorResponseDto> validationErrors = JsonConvert.DeserializeObject<List<ValidationErrorResponseDto>>(content)!;

            validationErrors.Should().NotBeNull();
            validationErrors.Count.Should().BeGreaterThan(0);
        }

        [Fact(DisplayName = "Deve atualizar status do pedido com sucesso")]
        public void DeveAtualizarStatusDoPedidoComSucesso()
        {
            Faker _faker = new Faker("pt_BR");

            // Criar cliente
            var customerRequest = new CustomerRequestDto
            {
                Name = _faker.Person.FullName,
                Email = _faker.Person.Email,
                Phone = _faker.Phone.PhoneNumberFormat(0)
            };

            var responseCustomer = _httpClient.PostAsJsonAsync("api/customers", customerRequest).Result;

            responseCustomer.StatusCode.Should().Be(HttpStatusCode.Created);

            var customerObj = JsonConvert.DeserializeObject<dynamic>(responseCustomer.Content.ReadAsStringAsync().Result);

            CustomerResponseDto customerResponse = JsonConvert.DeserializeObject<CustomerResponseDto>(customerObj.data.ToString());

            // Criar produto
            var productRequest = new ProductRequestDto
            {
                Name = _faker.Commerce.ProductName(),
                Price = Convert.ToDecimal(_faker.Commerce.Price()),
                Quantity = 10
            };

            var responseProduct = _httpClient.PostAsJsonAsync("api/products", productRequest).Result;

            responseProduct.StatusCode.Should().Be(HttpStatusCode.Created);

            var productObj = JsonConvert.DeserializeObject<dynamic>(responseProduct.Content.ReadAsStringAsync().Result);

            ProductResponseDto productResponse = JsonConvert.DeserializeObject<ProductResponseDto>(productObj.data.ToString());

            // Criar pedido
            var orderRequest = new CreateOrderRequestDto
            {
                CustomerId = customerResponse.Id,
                OrderItems = new List<CreateOrderItemRequestDto>
                {
                    new CreateOrderItemRequestDto { ProductId = productResponse.Id, Quantity = 1 }
                }
            };

            var responseOrder = _httpClient.PostAsJsonAsync("api/orders", orderRequest).Result;

            responseOrder.StatusCode.Should().Be(HttpStatusCode.Created);

            var orderObj = JsonConvert.DeserializeObject<dynamic>(responseOrder.Content.ReadAsStringAsync().Result);

            OrderResponseDto orderResponse = JsonConvert.DeserializeObject<OrderResponseDto>((orderObj.data ?? orderObj.Data).ToString());

            // Atualizar status do pedido
            var updateRequest = new UpdateOrderStatusRequestDto 
            { 
                Status = OrderStatus.Approved
            }; 

            var responseUpdate = _httpClient.PatchAsJsonAsync($"api/orders/{orderResponse.Id}/status", updateRequest).Result;

            responseUpdate.StatusCode.Should().Be(HttpStatusCode.OK);

            var contentUpdate = responseUpdate.Content.ReadAsStringAsync().Result;

            var objetoUpdate = JsonConvert.DeserializeObject<dynamic>(contentUpdate);

            string message = objetoUpdate.message?.ToString();

            message.Should().Be("Status do pedido atualizado com sucesso");

            OrderResponseDto updatedOrder = JsonConvert.DeserializeObject<OrderResponseDto>(objetoUpdate.data.ToString());

            updatedOrder.Should().NotBeNull();
            updatedOrder!.OrderStatus.Should().NotBeNullOrWhiteSpace();
            updatedOrder.OrderStatus.Should().Be("Aprovado");
        }

        [Fact(DisplayName = "Deve retornar erro ao atualizar status com dados invalidos")]
        public void DeveRetornarErroAoAtualizarStatusComDadosInvalidos()
        {
            Faker _faker = new Faker("pt_BR");

            // Criar cliente e produto e pedido mínimo para ter um pedido válido
            var customerRequest = new CustomerRequestDto
            {
                Name = _faker.Person.FullName,
                Email = _faker.Person.Email,
                Phone = _faker.Phone.PhoneNumberFormat(0)
            };

            var responseCustomer = _httpClient.PostAsJsonAsync("api/customers", customerRequest).Result;

            responseCustomer.StatusCode.Should().Be(HttpStatusCode.Created);

            var customerObj = JsonConvert.DeserializeObject<dynamic>(responseCustomer.Content.ReadAsStringAsync().Result);

            CustomerResponseDto customerResponse = JsonConvert.DeserializeObject<CustomerResponseDto>(customerObj.data.ToString());

            var productRequest = new ProductRequestDto
            {
                Name = _faker.Commerce.ProductName(),
                Price = Convert.ToDecimal(_faker.Commerce.Price()),
                Quantity = 10
            };

            var responseProduct = _httpClient.PostAsJsonAsync("api/products", productRequest).Result;

            responseProduct.StatusCode.Should().Be(HttpStatusCode.Created);

            var productObj = JsonConvert.DeserializeObject<dynamic>(responseProduct.Content.ReadAsStringAsync().Result);

            ProductResponseDto productResponse = JsonConvert.DeserializeObject<ProductResponseDto>(productObj.data.ToString());

            var orderRequest = new CreateOrderRequestDto
            {
                CustomerId = customerResponse.Id,
                OrderItems = new List<CreateOrderItemRequestDto> 
                { 
                    new CreateOrderItemRequestDto 
                    { 
                        ProductId = productResponse.Id, 
                        Quantity = 1 
                    } 
                }
            };

            var responseOrder = _httpClient.PostAsJsonAsync("api/orders", orderRequest).Result;

            responseOrder.StatusCode.Should().Be(HttpStatusCode.Created);

            var content = responseOrder.Content.ReadAsStringAsync().Result;

            var orderObj = JsonConvert.DeserializeObject<dynamic>(content);

            OrderResponseDto orderResponse = JsonConvert.DeserializeObject<OrderResponseDto>(orderObj.data.ToString());


            // Enviar status inválido
            var invalidUpdate = new UpdateOrderStatusRequestDto 
            { 
                Status = (OrderStatus)999 
            };

            var responseUpdate = _httpClient.PatchAsJsonAsync($"api/orders/{orderResponse.Id}/status", invalidUpdate).Result;

            responseUpdate.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);

            var contentUpdated = responseUpdate.Content.ReadAsStringAsync().Result;

            var validationErrors = JsonConvert.DeserializeObject<List<ValidationErrorResponseDto>>(contentUpdated);

            validationErrors.Should().NotBeNull();
            validationErrors.Count.Should().BeGreaterThan(0);
        }

        [Fact(DisplayName = "Deve cancelar pedido com sucesso")]
        public void DeveCancelarPedidoComSucesso()
        {
            Faker _faker = new Faker("pt_BR");

            // Criar cliente
            var customerRequest = new CustomerRequestDto
            {
                Name = _faker.Person.FullName,
                Email = _faker.Person.Email,
                Phone = _faker.Phone.PhoneNumberFormat(0)
            };

            var responseCustomer = _httpClient.PostAsJsonAsync("api/customers", customerRequest).Result;

            responseCustomer.StatusCode.Should().Be(HttpStatusCode.Created);
            
            var customerObj = JsonConvert.DeserializeObject<dynamic>(responseCustomer.Content.ReadAsStringAsync().Result);

            CustomerResponseDto customerResponse = JsonConvert.DeserializeObject<CustomerResponseDto>(customerObj.data.ToString());

            // Criar produto
            var productRequest = new ProductRequestDto
            {
                Name = _faker.Commerce.ProductName(),
                Price = Convert.ToDecimal(_faker.Commerce.Price()),
                Quantity = 10
            };

            var responseProduct = _httpClient.PostAsJsonAsync("api/products", productRequest).Result;

            responseProduct.StatusCode.Should().Be(HttpStatusCode.Created);

            var content = responseProduct.Content.ReadAsStringAsync().Result;

            var productObj = JsonConvert.DeserializeObject<dynamic>(content);

            ProductResponseDto productResponse = JsonConvert.DeserializeObject<ProductResponseDto>(productObj.data.ToString());


            // Criar pedido
            var orderRequest = new CreateOrderRequestDto
            {
                CustomerId = customerResponse.Id,
                OrderItems = new List<CreateOrderItemRequestDto>
                {
                    new CreateOrderItemRequestDto
                    { 
                        ProductId = productResponse.Id, 
                        Quantity = 1 
                    }
                }
            };

            var responseOrder = _httpClient.PostAsJsonAsync("api/orders", orderRequest).Result;

            responseOrder.StatusCode.Should().Be(HttpStatusCode.Created);

            var orderObj = JsonConvert.DeserializeObject<dynamic>(responseOrder.Content.ReadAsStringAsync().Result);
            OrderResponseDto orderResponse = JsonConvert.DeserializeObject<OrderResponseDto>((orderObj.data ?? orderObj.Data).ToString());

            var updateRequest = new UpdateOrderStatusRequestDto
            {
                Status = OrderStatus.Cancelled,
            };

            // Cancelar pedido
            var responseCancel = _httpClient.PatchAsJsonAsync($"api/orders/{orderResponse.Id}/status", updateRequest).Result;
            responseCancel.StatusCode.Should().Be(HttpStatusCode.OK);

            var contentCancel = responseCancel.Content.ReadAsStringAsync().Result;

            var objetoCancel = JsonConvert.DeserializeObject<dynamic>(contentCancel);

            string message = objetoCancel.message?.ToString();

            message.Should().Be("Pedido cancelado com sucesso");

            OrderResponseDto canceledOrder = JsonConvert.DeserializeObject<OrderResponseDto>(objetoCancel.data.ToString());
            canceledOrder.OrderStatus.Should().Be("Cancelado");
        }

        [Fact(DisplayName = "Deve retornar erro ao cancelar pedido inexistente")]
        public void DeveRetornarErroAoCancelarPedidoInexistente()
        {
            var randomId = Guid.NewGuid();

            var updateRequest = new UpdateOrderStatusRequestDto
            {
                Status = OrderStatus.Cancelled,
            };

            var response = _httpClient.PatchAsJsonAsync($"api/orders/{randomId}/status", updateRequest).Result;

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var content = response.Content.ReadAsStringAsync().Result;

            var errorResponse = JsonConvert.DeserializeObject<ErrorResponseDto>(content);

            errorResponse.Should().NotBeNull();
            errorResponse!.Message.Should().NotBeNullOrWhiteSpace();
            errorResponse!.Message.Should().Be("O pedido com este Id não existe!");
        }

        [Fact(DisplayName = "Deve retornar erro ao cancelar pedido já cancelado")]
        public void DeveRetornarErroAoCancelarPedidoJaCancelado()
        {
            Faker _faker = new Faker("pt_BR");

            // Criar cliente, produto e pedido
            var customerRequest = new CustomerRequestDto
            {
                Name = _faker.Person.FullName,
                Email = _faker.Person.Email,
                Phone = _faker.Phone.PhoneNumberFormat(0)
            };

            var responseCustomer = _httpClient.PostAsJsonAsync("api/customers", customerRequest).Result;

            responseCustomer.StatusCode.Should().Be(HttpStatusCode.Created);

            var customerObj = JsonConvert.DeserializeObject<dynamic>(responseCustomer.Content.ReadAsStringAsync().Result);

            CustomerResponseDto customerResponse = JsonConvert.DeserializeObject<CustomerResponseDto>(customerObj.data.ToString());

            var productRequest = new ProductRequestDto
            {
                Name = _faker.Commerce.ProductName(),
                Price = Convert.ToDecimal(_faker.Commerce.Price()),
                Quantity = 10
            };

            var responseProduct = _httpClient.PostAsJsonAsync("api/products", productRequest).Result;

            responseProduct.StatusCode.Should().Be(HttpStatusCode.Created);

            var productObj = JsonConvert.DeserializeObject<dynamic>(responseProduct.Content.ReadAsStringAsync().Result);

            ProductResponseDto productResponse = JsonConvert.DeserializeObject<ProductResponseDto>(productObj.data.ToString());

            var orderRequest = new CreateOrderRequestDto
            {
                CustomerId = customerResponse.Id,
                OrderItems = new List<CreateOrderItemRequestDto> 
                { 
                    new CreateOrderItemRequestDto 
                    { 
                        ProductId = productResponse.Id, 
                        Quantity = 1 
                    } 
                }
            };

            var responseOrder = _httpClient.PostAsJsonAsync("api/orders", orderRequest).Result;

            responseOrder.StatusCode.Should().Be(HttpStatusCode.Created);

            var content = responseOrder.Content.ReadAsStringAsync().Result;

            var orderObj = JsonConvert.DeserializeObject<dynamic>(content);

            OrderResponseDto orderResponse = JsonConvert.DeserializeObject<OrderResponseDto>(orderObj.data.ToString());

            // Cancelar primeira vez
            var updateRequest = new UpdateOrderStatusRequestDto
            {
                Status = OrderStatus.Cancelled,
            };

            var responseCancel1 = _httpClient.PatchAsJsonAsync($"api/orders/{orderResponse.Id}/status", updateRequest).Result;

            responseCancel1.StatusCode.Should().Be(HttpStatusCode.OK);

            // Cancelar segunda vez -> deve falhar
            var responseCancel2 = _httpClient.PatchAsJsonAsync($"api/orders/{orderResponse.Id}/status", updateRequest).Result;

            responseCancel2.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var contentError = responseCancel2.Content.ReadAsStringAsync().Result;

            var errorResponse = JsonConvert.DeserializeObject<ErrorResponseDto>(contentError);

            errorResponse.Should().NotBeNull();
            errorResponse!.Message.Should().NotBeNullOrWhiteSpace();
            errorResponse!.Message.Should().Be("Não será possível atualizar os status do pedido pois este pedido foi cancelado!");
        }

        [Fact(DisplayName = "Deve retornar uma lista de pedidos com sucesso")]
        public void DeveRetornarUmaListaDePedidosComSucesso()
        {
            Faker _faker = new Faker("pt_BR");

            // Criar cliente e produto únicos
            var customerRequest = new CustomerRequestDto
            {
                Name = _faker.Person.FullName,
                Email = _faker.Person.Email,
                Phone = _faker.Phone.PhoneNumberFormat(0)
            };

            var responseCustomer = _httpClient.PostAsJsonAsync("api/customers", customerRequest).Result;

            responseCustomer.StatusCode.Should().Be(HttpStatusCode.Created);

            var customerObj = JsonConvert.DeserializeObject<dynamic>(responseCustomer.Content.ReadAsStringAsync().Result);

            CustomerResponseDto customerResponse = JsonConvert.DeserializeObject<CustomerResponseDto>((customerObj.data ?? customerObj.Data).ToString());

            var productRequest = new ProductRequestDto
            {
                Name = _faker.Commerce.ProductName(),
                Price = Convert.ToDecimal(_faker.Commerce.Price()),
                Quantity = 100
            };

            var responseProduct = _httpClient.PostAsJsonAsync("api/products", productRequest).Result;

            responseProduct.StatusCode.Should().Be(HttpStatusCode.Created);

            var productObj = JsonConvert.DeserializeObject<dynamic>(responseProduct.Content.ReadAsStringAsync().Result);

            ProductResponseDto productResponse = JsonConvert.DeserializeObject<ProductResponseDto>(productObj.data.ToString());

            // Criar múltiplos pedidos
            foreach (var _ in Enumerable.Range(1, 5))
            {
                var orderRequest = new CreateOrderRequestDto
                {
                    CustomerId = customerResponse.Id,
                    OrderItems = new List<CreateOrderItemRequestDto> 
                    { 
                        new CreateOrderItemRequestDto 
                        { 
                            ProductId = productResponse.Id, 
                            Quantity = 1 
                        } 
                    }
                };

                var responseOrder = _httpClient.PostAsJsonAsync("api/orders", orderRequest).Result;
                responseOrder.StatusCode.Should().Be(HttpStatusCode.Created);
            }

            var responseList = _httpClient.GetAsync($"api/orders?pageNumber=1&pageSize=10").Result;
            responseList.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = responseList.Content.ReadAsStringAsync().Result;
            List<OrderResponseDto> orders = JsonConvert.DeserializeObject<List<OrderResponseDto>>(content)!;

            orders.Should().NotBeNull();
            orders.Count.Should().BeGreaterThanOrEqualTo(5);
            orders.All(o => o.Id != Guid.Empty).Should().BeTrue();
            orders.All(o => o.CustomerId != Guid.Empty).Should().BeTrue();
            orders.All(o => !string.IsNullOrEmpty(o.OrderStatus)).Should().BeTrue();
        }

        [Fact(DisplayName = "Deve retornar StatusCode 400 para id mal formado na rota update-order-status")]
        public void DeveRetornarStatusCode400ParaIdMalFormadoNaRota()
        {
            // Atualizar status do pedido
            var updateRequest = new UpdateOrderStatusRequestDto
            {
                Status = OrderStatus.Approved
            };

            // Rota espera Guid no path; enviar 'abc' deve retornar 400
            var response = _httpClient.PatchAsJsonAsync($"api/orders/abc/status", updateRequest).Result;

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}