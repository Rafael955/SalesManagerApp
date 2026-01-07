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
    public class ProductsControllerUnitTests
    {
        [Fact(DisplayName = "Post deve retornar 500 quando o service lança Exception")]
        public void Post_DeveRetornarStatus500_QuandoServiceLancaException()
        {
            var mockService = new Mock<IProductDomainService>();

            mockService
                .Setup(s => s.CriarProduto(It.IsAny<ProductRequestDto>()))
                .Throws(new Exception("Erro no service"));

            var controller = new ProductsController(mockService.Object);

            var request = new ProductRequestDto
            {
                Name = "Produto Teste",
                Price = 100.0m,
                Quantity = 10
            };

            var response = controller.Post(request) as ObjectResult;

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);

            var errorResponse = response.Value as ErrorResponseDto;

            errorResponse.Should().NotBeNull();
            errorResponse!.Message.Should().Be("Erro no service");
        }

        [Fact(DisplayName = "Post deve retornar 400 quando o service lança ApplicationException")]
        public void Post_DeveRetornarStatus400_QuandoServiceLancaApplicationException()
        {
            var mockService = new Mock<IProductDomainService>();

            mockService
                .Setup(s => s.CriarProduto(It.IsAny<ProductRequestDto>()))
                .Throws(new ApplicationException("Erro de aplicação"));

            var controller = new ProductsController(mockService.Object);

            var request = new ProductRequestDto
            {
                Name = "Produto Teste",
                Price = 100.0m,
                Quantity = 10
            };

            var response = controller.Post(request) as ObjectResult;

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

            var errorResponse = response.Value as ErrorResponseDto;

            errorResponse.Should().NotBeNull();
            errorResponse!.Message.Should().Be("Erro de aplicação");
        }

        [Fact(DisplayName = "Post deve retornar 422 quando o service lança ValidationException")]
        public void Post_DeveRetornarStatus422_QuandoServiceLancaValidationException()
        {
            var mockService = new Mock<IProductDomainService>();

            var validationErrors = new List<ValidationFailure>
            {
                new ValidationFailure("Name", "O nome do produto é obrigatório."),
                new ValidationFailure("Price", "O preço do produto deve ser maior que zero.")
            };

            mockService
                .Setup(s => s.CriarProduto(It.IsAny<ProductRequestDto>()))
                .Throws(new ValidationException(validationErrors));

            var controller = new ProductsController(mockService.Object);

            var request = new ProductRequestDto
            {
                Name = "",
                Price = 0.0m,
                Quantity = 10
            };

            var response = controller.Post(request) as ObjectResult;

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be(StatusCodes.Status422UnprocessableEntity);

            var errors = response.Value as IEnumerable<ValidationErrorResponseDto>;

            errors.Should().NotBeNull();
            errors!.Count().Should().Be(2);
            errors.Should().Contain(e => e.PropertyName == "Name" && e.ErrorMessage == "O nome do produto é obrigatório.");
            errors.Should().Contain(e => e.PropertyName == "Price" && e.ErrorMessage == "O preço do produto deve ser maior que zero.");
        }

        [Fact(DisplayName = "Post deve retornar 201 quando o service retorna sucesso")]
        public void Post_DeveRetornarStatus201_QuandoServiceRetornaSucesso()
        {
            var mockService = new Mock<IProductDomainService>();

            var productResponse = new ProductResponseDto
            {
                Id = Guid.NewGuid(),
                Name = "Produto OK",
                Price = 50.0m,
                Quantity = 5
            };

            mockService
                .Setup(s => s.CriarProduto(It.IsAny<ProductRequestDto>()))
                .Returns(productResponse);

            var controller = new ProductsController(mockService.Object);

            var request = new ProductRequestDto
            {
                Name = "Produto OK",
                Price = 50.0m,
                Quantity = 5
            };

            var objectResult = controller.Post(request) as ObjectResult;

            objectResult.Should().NotBeNull();
            objectResult!.StatusCode.Should().Be(StatusCodes.Status201Created);

            var value = objectResult.Value!;
            var messageProp = value.GetType().GetProperty("Message");
            var dataProp = value.GetType().GetProperty("Data");

            messageProp.Should().NotBeNull();
            dataProp.Should().NotBeNull();

            messageProp!.GetValue(value)!.Should().Be("Produto criado com sucesso");

            var dataValue = dataProp!.GetValue(value) as ProductResponseDto;
            dataValue.Should().NotBeNull();
            dataValue!.Id.Should().Be(productResponse.Id);
            dataValue.Name.Should().Be(productResponse.Name);
        }

        [Fact(DisplayName = "Put deve retornar 200 quando o service retorna sucesso")]
        public void Put_DeveRetornarStatus200_QuandoServiceRetornaSucesso()
        {
            var mockService = new Mock<IProductDomainService>();

            var productResponse = new ProductResponseDto
            {
                Id = Guid.NewGuid(),
                Name = "Produto Alterado",
                Price = 75.0m,
                Quantity = 3
            };

            mockService
                .Setup(s => s.AtualizarProduto(It.IsAny<Guid?>(), It.IsAny<ProductRequestDto>()))
                .Returns(productResponse);

            var controller = new ProductsController(mockService.Object);

            var request = new ProductRequestDto
            {
                Name = "Produto Alterado",
                Price = 75.0m,
                Quantity = 3
            };

            var objectResult = controller.Put(productResponse.Id, request) as ObjectResult;

            objectResult.Should().NotBeNull();
            objectResult!.StatusCode.Should().Be(StatusCodes.Status200OK);

            var value = objectResult.Value!;
            var messageProp = value.GetType().GetProperty("Message");
            var dataProp = value.GetType().GetProperty("Data");

            messageProp.Should().NotBeNull();
            dataProp.Should().NotBeNull();

            messageProp!.GetValue(value)!.Should().Be("Produto alterado com sucesso");

            var dataValue = dataProp!.GetValue(value) as ProductResponseDto;
            dataValue.Should().NotBeNull();
            dataValue!.Id.Should().Be(productResponse.Id);
            dataValue.Name.Should().Be(productResponse.Name);
        }

        [Fact(DisplayName = "Put deve retornar 422 quando o service lança ValidationException")]
        public void Put_DeveRetornarStatus422_QuandoServiceLancaValidationException()
        {
            var failures = new List<ValidationFailure>
            {
                new ValidationFailure("Name", "O nome do produto é obrigatório.")
            };

            var validationException = new ValidationException(failures);

            var mockService = new Mock<IProductDomainService>();

            mockService
                .Setup(s => s.AtualizarProduto(It.IsAny<Guid?>(), It.IsAny<ProductRequestDto>()))
                .Throws(validationException);

            var controller = new ProductsController(mockService.Object);

            var request = new ProductRequestDto
            {
                Name = "",
                Price = 10m,
                Quantity = 1
            };

            var objectResult = controller.Put(Guid.NewGuid(), request) as ObjectResult;

            objectResult.Should().NotBeNull();
            objectResult!.StatusCode.Should().Be(StatusCodes.Status422UnprocessableEntity);

            var errors = objectResult.Value as IEnumerable<ValidationErrorResponseDto>;

            errors.Should().NotBeNull();
            errors!.Should().Contain(e => e.PropertyName == "Name" && e.ErrorMessage == "O nome do produto é obrigatório.");
        }

        [Fact(DisplayName = "Put deve retornar 400 quando o service lança ApplicationException")]
        public void Put_DeveRetornarStatus400_QuandoServiceLancaApplicationException()
        {
            var mockService = new Mock<IProductDomainService>();

            mockService
                .Setup(s => s.AtualizarProduto(It.IsAny<Guid?>(), It.IsAny<ProductRequestDto>()))
                .Throws(new ApplicationException("Produto não encontrado"));

            var controller = new ProductsController(mockService.Object);

            var request = new ProductRequestDto
            {
                Name = "X",
                Price = 1m,
                Quantity = 1
            };

            var objectResult = controller.Put(Guid.NewGuid(), request) as ObjectResult;

            objectResult.Should().NotBeNull();
            objectResult!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

            var error = objectResult.Value as ErrorResponseDto;
            error.Should().NotBeNull();
            error!.Message.Should().Be("Produto não encontrado");
        }

        [Fact(DisplayName = "Delete deve retornar 204 quando o service executa com sucesso")]
        public void Delete_DeveRetornarStatus204_QuandoServiceRetornaSucesso()
        {
            var mockService = new Mock<IProductDomainService>();

            mockService
                .Setup(s => s.ExcluirProduto(It.IsAny<Guid?>()))
                .Verifiable();

            var controller = new ProductsController(mockService.Object);

            var id = Guid.NewGuid();
            var result = controller.Delete(id);

            var statusResult = result as StatusCodeResult;
            statusResult.Should().NotBeNull();
            statusResult!.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        }

        [Fact(DisplayName = "Delete deve retornar 400 quando o service lança ApplicationException")]
        public void Delete_DeveRetornarStatus400_QuandoServiceLancaApplicationException()
        {
            var mockService = new Mock<IProductDomainService>();

            mockService
                .Setup(s => s.ExcluirProduto(It.IsAny<Guid?>()))
                .Throws(new ApplicationException("Produto não existe"));

            var controller = new ProductsController(mockService.Object);

            var objectResult = controller.Delete(Guid.NewGuid()) as ObjectResult;

            objectResult.Should().NotBeNull();
            objectResult!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

            var error = objectResult.Value as ErrorResponseDto;
            error.Should().NotBeNull();
            error!.Message.Should().Be("Produto não existe");
        }

        [Fact(DisplayName = "GetById deve retornar 200 quando o service retorna sucesso")]
        public void GetById_DeveRetornarStatus200_QuandoServiceRetornaSucesso()
        {
            var mockService = new Mock<IProductDomainService>();

            var productResponse = new ProductResponseDto
            {
                Id = Guid.NewGuid(),
                Name = "Produto Get",
                Price = 20m,
                Quantity = 2
            };

            mockService
                .Setup(s => s.ObterProdutoPorId(It.IsAny<Guid?>()))
                .Returns(productResponse);

            var controller = new ProductsController(mockService.Object);

            var objectResult = controller.GetById(productResponse.Id) as ObjectResult;

            objectResult.Should().NotBeNull();
            objectResult!.StatusCode.Should().Be(StatusCodes.Status200OK);

            var data = objectResult.Value as ProductResponseDto;
            data.Should().NotBeNull();
            data!.Id.Should().Be(productResponse.Id);
        }

        [Fact(DisplayName = "GetById deve retornar 400 quando o service lança ApplicationException")]
        public void GetById_DeveRetornarStatus400_QuandoServiceLancaApplicationException()
        {
            var mockService = new Mock<IProductDomainService>();

            mockService
                .Setup(s => s.ObterProdutoPorId(It.IsAny<Guid?>()))
                .Throws(new ApplicationException("Produto não existe"));

            var controller = new ProductsController(mockService.Object);

            var objectResult = controller.GetById(Guid.NewGuid()) as ObjectResult;

            objectResult.Should().NotBeNull();
            objectResult!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

            var error = objectResult.Value as ErrorResponseDto;
            error.Should().NotBeNull();
            error!.Message.Should().Be("Produto não existe");
        }

        [Fact(DisplayName = "Get deve retornar 200 e lista quando o service retorna sucesso")]
        public void Get_DeveRetornarStatus200_QuandoServiceRetornaSucesso()
        {
            var mockService = new Mock<IProductDomainService>();

            var list = new List<ProductResponseDto>
            {
                new ProductResponseDto { Id = Guid.NewGuid(), Name = "P1", Price = 10m, Quantity = 1 },
                new ProductResponseDto { Id = Guid.NewGuid(), Name = "P2", Price = 20m, Quantity = 2 }
            };

            mockService
                .Setup(s => s.ObterTodosProdutos())
                .Returns(list);

            var controller = new ProductsController(mockService.Object);

            var objectResult = controller.Get() as ObjectResult;

            objectResult.Should().NotBeNull();
            objectResult!.StatusCode.Should().Be(StatusCodes.Status200OK);

            var returned = objectResult.Value as IEnumerable<ProductResponseDto>;
            returned.Should().NotBeNull();
            returned!.Count().Should().Be(2);
        }

        [Fact(DisplayName = "Get deve retornar 500 quando o service lança Exception")]
        public void Get_DeveRetornarStatus500_QuandoServiceLancaException()
        {
            var mockService = new Mock<IProductDomainService>();

            mockService
                .Setup(s => s.ObterTodosProdutos())
                .Throws(new Exception("Erro inesperado na listagem"));

            var controller = new ProductsController(mockService.Object);

            var objectResult = controller.Get() as ObjectResult;

            objectResult.Should().NotBeNull();
            objectResult!.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);

            var error = objectResult.Value as ErrorResponseDto;
            error.Should().NotBeNull();
            error!.Message.Should().Be("Erro inesperado na listagem");
        }
    }
}
