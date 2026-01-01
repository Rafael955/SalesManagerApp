using Azure.Core;
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
    public class OrdersController(IOrderDomainService orderDomainService) : ControllerBase
    {
        [HttpPost("create-order")]
        [ProducesResponseType(typeof(OrderResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(IEnumerable<ValidationErrorResponseDto>), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
        public IActionResult CreateOrder([FromBody] CreateOrderRequestDto request)
        {
            try
            {
                var result = orderDomainService.CriarPedido(request);

                return StatusCode(StatusCodes.Status201Created, new
                {
                    Message = "Pedido criado com sucesso",
                    Data = result
                });
            }
            catch (ValidationException ex)
            {
                var errors = ex.Errors.Select(err => new ValidationErrorResponseDto
                {
                    PropertyName = err.PropertyName,
                    ErrorMessage = err.ErrorMessage
                }).ToList();

                return StatusCode(StatusCodes.Status422UnprocessableEntity, new
                {
                    Message = "Erros de validação encontrados",
                    Errors = errors
                });
            }
            catch (ApplicationException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new
                {
                    ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    ex.Message
                });
            }
        }

        [HttpGet("update-order-status/{id}")]
        [ProducesResponseType(typeof(OrderResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<ValidationErrorResponseDto>), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateOrderStatus([FromRoute] Guid id, [FromBody] UpdateOrderStatusRequestDto request)
        {
            try
            {
                var result = orderDomainService.AtualizarStatusDoPedido(id, request);

                return StatusCode(StatusCodes.Status201Created, new
                {
                    Message = "Pedido criado com sucesso",
                    Data = result
                });
            }
            catch (ValidationException ex)
            {
                var errors = ex.Errors.Select(err => new ValidationErrorResponseDto
                {
                    PropertyName = err.PropertyName,
                    ErrorMessage = err.ErrorMessage
                }).ToList();

                return StatusCode(StatusCodes.Status422UnprocessableEntity, new
                {
                    Message = "Erros de validação encontrados",
                    Errors = errors
                });
            }
            catch (ApplicationException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new
                {
                    ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    ex.Message
                });
            }
        }

        [HttpDelete("cancel-order/{id}")]
        [ProducesResponseType(typeof(OrderResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
        public IActionResult CancelOrder(Guid? id)
        {
            try
            {
                var result = orderDomainService.CancelarPedido(id);

                return StatusCode(StatusCodes.Status200OK, new
                {
                    Message = "Pedido cancelado com sucesso",
                    Data = result
                });
            }
            catch (ApplicationException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new
                {
                    ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    ex.Message
                });
            }
        }

        [HttpGet("list-orders")]
        [ProducesResponseType(typeof(OrderResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
        public IActionResult ListOrders([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            try
            {
                var result = orderDomainService.ListarPedidos(pageNumber, pageSize);

                return StatusCode(StatusCodes.Status200OK, result);
            }
            catch (ApplicationException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new
                {
                    ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    ex.Message
                });
            }
        }
    }
}
