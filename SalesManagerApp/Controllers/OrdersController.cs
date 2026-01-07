using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesManagerApp.Domain.Dtos.Requests;
using SalesManagerApp.Domain.Dtos.Responses;
using SalesManagerApp.Domain.Enums;
using SalesManagerApp.Domain.Interfaces.Services;

namespace SalesManagerApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController(IOrderDomainService orderDomainService) : ControllerBase
    {
        [HttpPost]
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

        [HttpPatch("{id}/status")]
        [ProducesResponseType(typeof(OrderResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<ValidationErrorResponseDto>), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateOrderStatus([FromRoute] Guid id, [FromBody] UpdateOrderStatusRequestDto request)
        {
            try
            {
                var result = orderDomainService.AtualizarStatusDoPedido(id, request);

                return StatusCode(StatusCodes.Status200OK, new
                {
                    Message = request.Status == OrderStatus.Cancelled ? "Pedido cancelado com sucesso" : "Status do pedido atualizado com sucesso",
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

        [HttpGet]
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
        [ProducesResponseType(typeof(OrderResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
        public IActionResult GetOrderById(Guid id)
        {
            try
            {
                var result = orderDomainService.ObterPedidoPorId(id);

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
    }
}
