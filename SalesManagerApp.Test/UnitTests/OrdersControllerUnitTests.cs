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

namespace SalesManagerApp.Test.UnitTests
{
    public class OrdersControllerUnitTests
    {
        [Fact(DisplayName = "Post deve retornar 500 quando o service lança Exception")]
        public void Post_DeveRetornarStatus500_QuandoServiceLancaException()
        {
            var mockService = new Mock<IOrderDomainService>();

            mockService
                .Setup(s => s.CriarPedido(It.IsAny<CreateOrderRequestDto>()))
                .Throws(new Exception("Erro inesperado no serviço"));

            var controller = new OrdersController(mockService.Object);

            var request = new CreateOrderRequestDto
            {
                CustomerId = Guid.NewGuid(),
                OrderItems = new List<CreateOrderItemRequestDto>()
            };

            request.OrderItems.Add(new CreateOrderItemRequestDto
            {
                ProductId = Guid.NewGuid(),
                Quantity = 20
            });

            var result = controller.CreateOrder(request);

            var objectResult = result as ObjectResult;

            objectResult.Should().NotBeNull();
            objectResult!.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);

            var error = objectResult.Value as ErrorResponseDto;

            error.Should().NotBeNull();
            error!.Message.Should().Be("Erro inesperado no serviço");
        }

        [Fact(DisplayName = "Post deve retornar 400 quando o service lança ApplicationException")]
        public void Post_DeveRetornarStatus400_QuandoServiceLancaApplicationException()
        {
            var mockService = new Mock<IOrderDomainService>();

            mockService
                .Setup(s => s.CriarPedido(It.IsAny<CreateOrderRequestDto>()))
                .Throws(new ApplicationException("O pedido com este Id não existe!"));

            var controller = new OrdersController(mockService.Object);

            var request = new CreateOrderRequestDto
            {
                CustomerId = Guid.NewGuid(),
                OrderItems = new List<CreateOrderItemRequestDto>()
            };

            request.OrderItems.Add(new CreateOrderItemRequestDto
            {
                ProductId = Guid.NewGuid(),
                Quantity = 20
            });

            var result = controller.CreateOrder(request);

            var objectResult = result as ObjectResult;

            objectResult.Should().NotBeNull();
            objectResult!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

            var error = objectResult.Value as ErrorResponseDto;

            error.Should().NotBeNull();
            error!.Message.Should().Be("O pedido com este Id não existe!");
        }

        [Fact(DisplayName = "Post deve retornar 422 quando o service lança ValidationException")]
        public void Post_DeveRetornarStatus422_QuandoServiceLancaValidationException()
        {
            var failures = new List<ValidationFailure>
            {
                new ValidationFailure("CustomerId", "CustomerId inválido")
            };

            var validationException = new ValidationException(failures);

            var mockService = new Mock<IOrderDomainService>();

            mockService
                .Setup(s => s.CriarPedido(It.IsAny<CreateOrderRequestDto>()))
                .Throws(validationException);

            var controller = new OrdersController(mockService.Object);

            var request = new CreateOrderRequestDto
            {
                CustomerId = Guid.NewGuid(),
                OrderItems = new List<CreateOrderItemRequestDto>()
            };

            request.OrderItems.Add(new CreateOrderItemRequestDto
            {
                ProductId = Guid.NewGuid(),
                Quantity = 20
            });

            var result = controller.CreateOrder(request);

            var objectResult = result as ObjectResult;

            objectResult.Should().NotBeNull();

            objectResult!.StatusCode.Should().Be(StatusCodes.Status422UnprocessableEntity);

            var error = objectResult.Value as IEnumerable<ValidationErrorResponseDto>;

            error.Should().NotBeNull();
            error!.Should().Contain(e => e.PropertyName == "CustomerId" && e.ErrorMessage == "CustomerId inválido");
        }

        [Fact(DisplayName = "Post deve retornar 201 quando o service retorna sucesso")]
        public void Post_DeveRetornarStatus201_QuandoServiceRetornaSucesso()
        {
            var mockService = new Mock<IOrderDomainService>();

            var orderResponse = new OrderResponseDto
            {
                Id = Guid.NewGuid(),
                OrderDate = DateTime.UtcNow,
                TotalValue = 100m,
                OrderStatus = "Pending",
                CustomerId = Guid.NewGuid()
            };

            mockService
                .Setup(s => s.CriarPedido(It.IsAny<CreateOrderRequestDto>()))
                .Returns(orderResponse);

            var controller = new OrdersController(mockService.Object);

            var request = new CreateOrderRequestDto
            {
                CustomerId = Guid.NewGuid(),
                OrderItems = new List<CreateOrderItemRequestDto>
                {
                    new CreateOrderItemRequestDto { ProductId = Guid.NewGuid(), Quantity = 1 }
                }
            };

            var result = controller.CreateOrder(request);

            var objectResult = result as ObjectResult;

            objectResult.Should().NotBeNull();
            objectResult!.StatusCode.Should().Be(StatusCodes.Status201Created);

            var value = objectResult.Value!;
            var messageProp = value.GetType().GetProperty("Message");
            var dataProp = value.GetType().GetProperty("Data");

            messageProp.Should().NotBeNull();
            dataProp.Should().NotBeNull();

            messageProp!.GetValue(value)!.Should().Be("Pedido criado com sucesso");

            var dataValue = dataProp!.GetValue(value) as OrderResponseDto;
            dataValue.Should().NotBeNull();
            dataValue!.Id.Should().Be(orderResponse.Id);
        }

        [Fact(DisplayName = "UpdateOrderStatus deve retornar 201 quando service retorna sucesso")]
        public void UpdateOrderStatus_DeveRetornarStatus201_QuandoServiceRetornaSucesso()
        {
            var mockService = new Mock<IOrderDomainService>();

            var orderResponse = new OrderResponseDto
            {
                Id = Guid.NewGuid(),
                OrderDate = DateTime.UtcNow,
                TotalValue = 50m,
                OrderStatus = "Processing",
                CustomerId = Guid.NewGuid()
            };

            mockService
                .Setup(s => s.AtualizarStatusDoPedido(It.IsAny<Guid?>(), It.IsAny<UpdateOrderStatusRequestDto>()))
                .Returns(orderResponse);

            var controller = new OrdersController(mockService.Object);

            var request = new UpdateOrderStatusRequestDto { OrderStatus = 2 };

            var result = controller.UpdateOrderStatus(orderResponse.Id, request);

            var objectResult = result as ObjectResult;

            objectResult.Should().NotBeNull();
            objectResult!.StatusCode.Should().Be(StatusCodes.Status201Created);

            var value = objectResult.Value!;
            var messageProp = value.GetType().GetProperty("Message");
            var dataProp = value.GetType().GetProperty("Data");

            messageProp.Should().NotBeNull();
            dataProp.Should().NotBeNull();

            messageProp!.GetValue(value)!.Should().Be("Pedido criado com sucesso");

            var dataValue = dataProp!.GetValue(value) as OrderResponseDto;
            dataValue.Should().NotBeNull();
            dataValue!.Id.Should().Be(orderResponse.Id);
        }

        [Fact(DisplayName = "UpdateOrderStatus deve retornar 422 quando service lança ValidationException")]
        public void UpdateOrderStatus_DeveRetornarStatus422_QuandoServiceLancaValidationException()
        {
            var failures = new List<ValidationFailure>
            {
                new ValidationFailure("OrderStatus", "Status inválido")
            };

            var validationException = new ValidationException(failures);

            var mockService = new Mock<IOrderDomainService>();

            mockService
                .Setup(s => s.AtualizarStatusDoPedido(It.IsAny<Guid?>(), It.IsAny<UpdateOrderStatusRequestDto>()))
                .Throws(validationException);

            var controller = new OrdersController(mockService.Object);

            var result = controller.UpdateOrderStatus(Guid.NewGuid(), new UpdateOrderStatusRequestDto { OrderStatus = 999 });

            var objectResult = result as ObjectResult;

            objectResult.Should().NotBeNull();
            objectResult!.StatusCode.Should().Be(StatusCodes.Status422UnprocessableEntity);

            var error = objectResult.Value as IEnumerable<ValidationErrorResponseDto>;

            error.Should().NotBeNull();
            error!.Should().Contain(e => e.PropertyName == "OrderStatus" && e.ErrorMessage == "Status inválido");
        }

        [Fact(DisplayName = "CancelOrder deve retornar 200 quando service retorna sucesso")]
        public void CancelOrder_DeveRetornarStatus200_QuandoServiceRetornaSucesso()
        {
            var mockService = new Mock<IOrderDomainService>();

            var orderResponse = new OrderResponseDto
            {
                Id = Guid.NewGuid(),
                OrderDate = DateTime.UtcNow,
                TotalValue = 30m,
                OrderStatus = "Cancelled",
                CustomerId = Guid.NewGuid()
            };

            mockService
                .Setup(s => s.CancelarPedido(It.IsAny<Guid?>()))
                .Returns(orderResponse);

            var controller = new OrdersController(mockService.Object);

            var result = controller.CancelOrder(orderResponse.Id);

            var objectResult = result as ObjectResult;

            objectResult.Should().NotBeNull();
            objectResult!.StatusCode.Should().Be(StatusCodes.Status200OK);

            var value = objectResult.Value!;
            var messageProp = value.GetType().GetProperty("Message");
            var dataProp = value.GetType().GetProperty("Data");

            messageProp.Should().NotBeNull();
            dataProp.Should().NotBeNull();

            messageProp!.GetValue(value)!.Should().Be("Pedido cancelado com sucesso");

            var dataValue = dataProp!.GetValue(value) as OrderResponseDto;
            dataValue.Should().NotBeNull();
            dataValue!.Id.Should().Be(orderResponse.Id);
        }

        [Fact(DisplayName = "CancelOrder deve retornar 400 quando service lança ApplicationException")]
        public void CancelOrder_DeveRetornarStatus400_QuandoServiceLancaApplicationException()
        {
            var mockService = new Mock<IOrderDomainService>();

            mockService
                .Setup(s => s.CancelarPedido(It.IsAny<Guid?>()))
                .Throws(new ApplicationException("O pedido com este Id não existe!"));

            var controller = new OrdersController(mockService.Object);

            var result = controller.CancelOrder(Guid.NewGuid());

            var objectResult = result as ObjectResult;

            objectResult.Should().NotBeNull();
            objectResult!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

            var error = objectResult.Value as ErrorResponseDto;

            error.Should().NotBeNull();
            error!.Message.Should().Be("O pedido com este Id não existe!");
        }

        [Fact(DisplayName = "ListOrders deve retornar 200 com lista quando service retorna sucesso")]
        public void ListOrders_DeveRetornarStatus200_QuandoServiceRetornaSucesso()
        {
            var mockService = new Mock<IOrderDomainService>();

            var list = new List<OrderResponseDto>
            {
                new OrderResponseDto { Id = Guid.NewGuid(), OrderDate = DateTime.UtcNow, TotalValue = 10m, OrderStatus = "Pending", CustomerId = Guid.NewGuid() },
                new OrderResponseDto { Id = Guid.NewGuid(), OrderDate = DateTime.UtcNow, TotalValue = 20m, OrderStatus = "Completed", CustomerId = Guid.NewGuid() }
            };

            mockService
                .Setup(s => s.ListarPedidos(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(list);

            var controller = new OrdersController(mockService.Object);

            var result = controller.ListOrders(1, 10);

            var objectResult = result as ObjectResult;

            objectResult.Should().NotBeNull();
            objectResult!.StatusCode.Should().Be(StatusCodes.Status200OK);

            var returned = objectResult.Value as IEnumerable<OrderResponseDto>;
            returned.Should().NotBeNull();
            returned!.Count().Should().Be(2);
        }

        [Fact(DisplayName = "ListOrders deve retornar 500 quando service lança Exception")]
        public void ListOrders_DeveRetornarStatus500_QuandoServiceLancaException()
        {
            var mockService = new Mock<IOrderDomainService>();

            mockService
                .Setup(s => s.ListarPedidos(It.IsAny<int>(), It.IsAny<int>()))
                .Throws(new Exception("Erro inesperado na listagem"));

            var controller = new OrdersController(mockService.Object);

            var result = controller.ListOrders(1, 10);

            var objectResult = result as ObjectResult;

            objectResult.Should().NotBeNull();
            objectResult!.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);

            var error = objectResult.Value as ErrorResponseDto;
            error.Should().NotBeNull();
            error!.Message.Should().Be("Erro inesperado na listagem");
        }
    }
}
