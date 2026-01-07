using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesManagerApp.Domain.Dtos.Requests;
using SalesManagerApp.Domain.Dtos.Responses;
using SalesManagerApp.Domain.Interfaces.Services;

namespace SalesManagerApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IProductDomainService productDomainService) : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(ProductResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(IEnumerable<ValidationErrorResponseDto>), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
        public IActionResult CreateProduct([FromBody] ProductRequestDto request)
        {
            try
            {
                var result = productDomainService.CriarProduto(request);

                return StatusCode(StatusCodes.Status201Created, new
                {
                    Message = "Produto criado com sucesso",
                    Data = result
                });
            }
            catch (ValidationException ex)
            {
                var errors = ex.Errors.Select(e => new ValidationErrorResponseDto
                {
                    PropertyName = e.PropertyName,
                    ErrorMessage = e.ErrorMessage
                });

                return StatusCode(StatusCodes.Status422UnprocessableEntity, errors);
            }
            catch (ApplicationException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ErrorResponseDto
                {
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseDto
                {
                    Message = ex.Message
                });
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ProductResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<ValidationErrorResponseDto>), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateProduct([FromRoute] Guid id, [FromBody] ProductRequestDto request)
        {
            try
            {
                var result = productDomainService.AtualizarProduto(id, request);

                return StatusCode(StatusCodes.Status200OK, new
                {
                    Message = "Produto alterado com sucesso",
                    Data = result
                });
            }
            catch (ValidationException ex)
            {
                var errors = ex.Errors.Select(e => new ValidationErrorResponseDto
                {
                    PropertyName = e.PropertyName,
                    ErrorMessage = e.ErrorMessage
                });

                return StatusCode(StatusCodes.Status422UnprocessableEntity, errors);
            }
            catch (ApplicationException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ErrorResponseDto
                {
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseDto
                {
                    Message = ex.Message
                });
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteProduct([FromRoute] Guid id)
        {
            try
            {
                productDomainService.ExcluirProduto(id);

                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch (ApplicationException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ErrorResponseDto
                {
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseDto
                {
                    Message = ex.Message
                });
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
        public IActionResult GetProductById([FromRoute] Guid id)
        {
            try
            {
                var result = productDomainService.ObterProdutoPorId(id);

                return StatusCode(StatusCodes.Status200OK, result);
            }
            catch (ApplicationException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ErrorResponseDto
                {
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseDto
                {
                    Message = ex.Message
                });
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(ProductResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
        public IActionResult ListProducts()
        {
            try
            {
                var result = productDomainService.ObterTodosProdutos();

                return StatusCode(StatusCodes.Status200OK, result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseDto
                {
                    Message = ex.Message
                });
            }
        }

    }
}
