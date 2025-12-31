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
    public class CustomersController(ICustomerDomainService customerDomainService) : ControllerBase
    {
        [HttpPost("register-customer")]
        [ProducesResponseType(typeof(CustomerResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationErrorResponseDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
        public IActionResult Post([FromBody] CustomerRequestDto request)
        {
            try
            {
                var result = customerDomainService.RegistrarCliente(request);

                return StatusCode(StatusCodes.Status201Created, new
                {
                    Message = "Cliente cadastrado com sucesso!",
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

                return StatusCode(StatusCodes.Status400BadRequest, errors);
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

        [HttpPut("update-customer/{id}")]
        [ProducesResponseType(typeof(CustomerResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationErrorResponseDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
        public IActionResult Put([FromRoute] Guid id, [FromBody] CustomerRequestDto request)
        {
            try
            {
                var result = customerDomainService.AtualizarCliente(id, request);

                return StatusCode(StatusCodes.Status200OK, new
                {
                    Message = "Cliente alterado com sucesso!",
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

                return StatusCode(StatusCodes.Status400BadRequest, errors);
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

        [HttpDelete("delete-customer/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
        public IActionResult Delete([FromRoute] Guid id)
        {
            try
            {
                customerDomainService.ExcluirCliente(id);

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

        [HttpGet("get-customer/{id}")]
        [ProducesResponseType(typeof(CustomerResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
        public IActionResult GetById([FromRoute] Guid id)
        {
            try
            {
                var result = customerDomainService.ObterClientePorId(id);

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

        [HttpGet("list-customers")]
        [ProducesResponseType(typeof(CustomerResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
        public IActionResult Get()
        {
            try
            {
                var result = customerDomainService.ObterTodosClientes();

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
