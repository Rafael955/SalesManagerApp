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
    public class CustomersControllerUnitTests
    {
        [Fact(DisplayName = "Post deve retornar 500 quando o service lança Exception")]
        public void Post_Returns500_WhenServiceThrowsException()
        {
            var mockService = new Mock<ICustomerDomainService>();

            mockService
                .Setup(s => s.RegistrarCliente(It.IsAny<CustomerRequestDto>()))
                .Throws(new Exception("Erro de servidor"));

            var controller = new CustomersController(mockService.Object);

            var request = new CustomerRequestDto { Name = "Nome", Email = "a@b.com", Phone = "123" };

            var result = controller.Post(request);

            var objectResult = result as ObjectResult;

            objectResult.Should().NotBeNull();
            objectResult!.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);

            var error = objectResult.Value as ErrorResponseDto;

            error.Should().NotBeNull();
            error!.Message.Should().Be("Erro de servidor");
        }

        [Fact(DisplayName = "Post deve retornar 400 quando o service lança ApplicationException")]
        public void Post_Returns400_WhenServiceThrowsApplicationException()
        {
            var mockService = new Mock<ICustomerDomainService>();

            mockService
                .Setup(s => s.RegistrarCliente(It.IsAny<CustomerRequestDto>()))
                .Throws(new ApplicationException("Erro de regra de negócio"));

            var controller = new CustomersController(mockService.Object);

            var request = new CustomerRequestDto { Name = "Nome", Email = "a@b.com", Phone = "123" };

            var result = controller.Post(request);

            var objectResult = result as ObjectResult;
            objectResult.Should().NotBeNull();
            objectResult!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

            var error = objectResult.Value as ErrorResponseDto;
            error.Should().NotBeNull();
            error!.Message.Should().Be("Erro de regra de negócio");
        }

        [Fact(DisplayName = "Post deve retornar 422 quando o service lança ValidationException")]
        public void Post_Returns422_WhenServiceThrowsValidationException()
        {
            var failures = new List<ValidationFailure>
            {
                new ValidationFailure("Name", "Name inválido")
            };

            var validationException = new ValidationException(failures);

            var mockService = new Mock<ICustomerDomainService>();
            mockService
                .Setup(s => s.RegistrarCliente(It.IsAny<CustomerRequestDto>()))
                .Throws(validationException);

            var controller = new CustomersController(mockService.Object);

            var request = new CustomerRequestDto { Name = "", Email = "invalido", Phone = "" };

            var result = controller.Post(request);

            var objectResult = result as ObjectResult;
            objectResult.Should().NotBeNull();
            objectResult!.StatusCode.Should().Be(StatusCodes.Status422UnprocessableEntity);

            var errors = objectResult.Value as IEnumerable<ValidationErrorResponseDto>;
            errors.Should().NotBeNull();
            errors!.Should().ContainSingle(e => e.PropertyName == "Name" && e.ErrorMessage == "Name inválido");
        }

        [Fact(DisplayName = "Put deve retornar 500 quando o service lança Exception")]
        public void Put_Returns500_WhenServiceThrowsException()
        {
            var mockService = new Mock<ICustomerDomainService>();
            mockService
                .Setup(s => s.AtualizarCliente(It.IsAny<Guid>(), It.IsAny<CustomerRequestDto>()))
                .Throws(new Exception("Erro de servidor"));

            var controller = new CustomersController(mockService.Object);

            var request = new CustomerRequestDto { Name = "Nome", Email = "a@b.com", Phone = "123" };

            var result = controller.Put(Guid.NewGuid(), request);

            var objectResult = result as ObjectResult;
            objectResult.Should().NotBeNull();
            objectResult!.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);

            var error = objectResult.Value as ErrorResponseDto;
            error.Should().NotBeNull();
            error!.Message.Should().Be("Erro de servidor");
        }

        [Fact(DisplayName = "Delete deve retornar 400 quando o service lança ApplicationException")]
        public void Delete_Returns400_WhenServiceThrowsApplicationException()
        {
            var mockService = new Mock<ICustomerDomainService>();
            mockService
                .Setup(s => s.ExcluirCliente(It.IsAny<Guid>()))
                .Throws(new ApplicationException("Erro de regra de negócio"));

            var controller = new CustomersController(mockService.Object);

            var result = controller.Delete(Guid.NewGuid());

            var objectResult = result as ObjectResult;
            objectResult.Should().NotBeNull();
            objectResult!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

            var error = objectResult.Value as ErrorResponseDto;
            error.Should().NotBeNull();
            error!.Message.Should().Be("Erro de regra de negócio");
        }

        [Fact(DisplayName = "Get deve retornar 500 quando o service lança Exception")]
        public void Get_Returns500_WhenServiceThrowsException()
        {
            var mockService = new Mock<ICustomerDomainService>();
            mockService
                .Setup(s => s.ObterTodosClientes())
                .Throws(new Exception("Erro de servidor"));

            var controller = new CustomersController(mockService.Object);

            var result = controller.Get();

            var objectResult = result as ObjectResult;
            objectResult.Should().NotBeNull();
            objectResult!.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);

            var error = objectResult.Value as ErrorResponseDto;
            error.Should().NotBeNull();
            error!.Message.Should().Be("Erro de servidor");
        }
    }
}