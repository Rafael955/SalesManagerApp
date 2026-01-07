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
        public void Post_DeveRetornarStatus500_QuandoServiceLancaException()
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
        public void Post_DeveRetornarStatus400_QuandoServiceLancaApplicationException()
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
        public void Post_DeveRetornarStatus422_QuandoServiceLancaValidationException()
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
        public void Put_DeveRetornarStatus500_QuandoServiceLancaException()
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
        public void Delete_DeveRetornarStatus400_QuandoServiceLancaApplicationException()
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
        public void Get_DeveRetornarStatus500_QuandoServiceLancaException()
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

        [Fact(DisplayName = "Post deve retornar 201 quando o service retorna sucesso")]
        public void Post_DeveRetornarStatus201_QuandoServiceRetornaSucesso()
        {
            var mockService = new Mock<ICustomerDomainService>();

            var customerResponse = new CustomerResponseDto
            {
                Id = Guid.NewGuid(),
                Name = "Cliente Teste",
                Email = "teste@x.com",
                Phone = "99999"
            };

            mockService
                .Setup(s => s.RegistrarCliente(It.IsAny<CustomerRequestDto>()))
                .Returns(customerResponse);

            var controller = new CustomersController(mockService.Object);

            var request = new CustomerRequestDto { Name = "Cliente Teste", Email = "teste@x.com", Phone = "99999" };

            var result = controller.Post(request);

            var objectResult = result as ObjectResult;

            objectResult.Should().NotBeNull();
            objectResult!.StatusCode.Should().Be(StatusCodes.Status201Created);

            var value = objectResult.Value!;
            var messageProp = value.GetType().GetProperty("Message");
            var dataProp = value.GetType().GetProperty("Data");

            messageProp.Should().NotBeNull();
            dataProp.Should().NotBeNull();

            messageProp!.GetValue(value)!.Should().Be("Cliente cadastrado com sucesso!");

            var dataValue = dataProp!.GetValue(value) as CustomerResponseDto;

            dataValue.Should().NotBeNull();
            dataValue!.Id.Should().Be(customerResponse.Id);
            dataValue!.Name.Should().Be("Cliente Teste");
            dataValue!.Email.Should().Be("teste@x.com");
            dataValue!.Phone.Should().Be("99999");
        }

        [Fact(DisplayName = "Put deve retornar 200 quando o service retorna sucesso")]
        public void Put_DeveRetornarStatus200_QuandoServiceRetornaSucesso()
        {
            var mockService = new Mock<ICustomerDomainService>();

            var customerResponse = new CustomerResponseDto
            {
                Id = Guid.NewGuid(),
                Name = "Cliente Alterado",
                Email = "alterado@x.com",
                Phone = "11111"
            };

            mockService
                .Setup(s => s.AtualizarCliente(It.IsAny<Guid>(), It.IsAny<CustomerRequestDto>()))
                .Returns(customerResponse);

            var controller = new CustomersController(mockService.Object);

            var request = new CustomerRequestDto { Name = "Cliente Alterado", Email = "alterado@x.com", Phone = "11111" };

            var result = controller.Put(customerResponse.Id, request);

            var objectResult = result as ObjectResult;

            objectResult.Should().NotBeNull();
            objectResult!.StatusCode.Should().Be(StatusCodes.Status200OK);

            var value = objectResult.Value!;
            var messageProp = value.GetType().GetProperty("Message");
            var dataProp = value.GetType().GetProperty("Data");

            messageProp.Should().NotBeNull();
            dataProp.Should().NotBeNull();

            messageProp!.GetValue(value)!.Should().Be("Cliente alterado com sucesso!");
            
            var dataValue = dataProp!.GetValue(value) as CustomerResponseDto;
            
            dataValue.Should().NotBeNull();
            dataValue!.Id.Should().Be(customerResponse.Id);
            dataValue!.Name.Should().Be("Cliente Alterado");
            dataValue!.Email.Should().Be("alterado@x.com");
            dataValue!.Phone.Should().Be("11111");
        }

        [Fact(DisplayName = "Delete deve retornar 204 quando o service executa com sucesso")]
        public void Delete_DeveRetornarStatus204_QuandoServiceRetornaSucesso()
        {
            var mockService = new Mock<ICustomerDomainService>();

            // ExcluirCliente é void; não lançar exceção representa sucesso.
            mockService
                .Setup(s => s.ExcluirCliente(It.IsAny<Guid>()))
                .Verifiable();

            var controller = new CustomersController(mockService.Object);

            var id = Guid.NewGuid();
            var result = controller.Delete(id);

            var statusResult = result as StatusCodeResult;

            statusResult.Should().NotBeNull();
            statusResult!.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        }

        [Fact(DisplayName = "GetById deve retornar 200 e Customer quando service retorna sucesso")]
        public void GetById_DeveRetornarStatus200_QuandoServiceRetornaSucesso()
        {
            var mockService = new Mock<ICustomerDomainService>();

            var customerResponse = new CustomerResponseDto
            {
                Id = Guid.NewGuid(),
                Name = "Cliente GetById",
                Email = "get@x.com",
                Phone = "22222"
            };

            mockService
                .Setup(s => s.ObterClientePorId(It.IsAny<Guid>()))
                .Returns(customerResponse);

            var controller = new CustomersController(mockService.Object);

            var result = controller.GetById(customerResponse.Id);

            var objectResult = result as ObjectResult;

            objectResult.Should().NotBeNull();
            objectResult!.StatusCode.Should().Be(StatusCodes.Status200OK);

            var data = objectResult.Value as CustomerResponseDto;
            data.Should().NotBeNull();
            data!.Id.Should().Be(customerResponse.Id);
        }

        [Fact(DisplayName = "Get deve retornar 200 e lista de customers quando service retorna sucesso")]
        public void Get_DeveRetornarStatus200ELista_QuandoServiceRetornaSucesso()
        {
            var mockService = new Mock<ICustomerDomainService>();

            var list = new List<CustomerResponseDto>
            {
                new CustomerResponseDto { Id = Guid.NewGuid(), Name = "C1", Email = "c1@x.com", Phone = "1" },
                new CustomerResponseDto { Id = Guid.NewGuid(), Name = "C2", Email = "c2@x.com", Phone = "2" }
            };

            mockService
                .Setup(s => s.ObterTodosClientes())
                .Returns(list);

            var controller = new CustomersController(mockService.Object);

            var result = controller.Get();

            var objectResult = result as ObjectResult;

            objectResult.Should().NotBeNull();
            objectResult!.StatusCode.Should().Be(StatusCodes.Status200OK);

            var returned = objectResult.Value as IEnumerable<CustomerResponseDto>;
            returned.Should().NotBeNull();
            returned!.Count().Should().Be(2);
        }
    }
}