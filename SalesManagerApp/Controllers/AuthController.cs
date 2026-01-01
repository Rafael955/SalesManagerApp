using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using SalesManagerApp.Domain.Dtos.Requests;
using SalesManagerApp.Domain.Dtos.Responses;
using SalesManagerApp.Domain.Interfaces.Services;
using System.Security.Authentication;

namespace SalesManagerApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthDomainService authDomainService) : ControllerBase
    {
        [HttpPost("login")]
        [ProducesResponseType(typeof(UserLoginResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<ValidationErrorResponseDto>),StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ErrorResponseDto),StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponseDto),StatusCodes.Status500InternalServerError)]
        public IActionResult Login([FromBody] UserLoginRequestDto request)
        {
            try
            {
                var result = authDomainService.AutenticarUsuario(request);

                return StatusCode(StatusCodes.Status200OK, result);
            }
            catch(ValidationException ex)
            {
                var errors = ex.Errors.Select(e => new ValidationErrorResponseDto 
                {
                    PropertyName = e.PropertyName,
                    ErrorMessage = e.ErrorMessage 
                });

                return StatusCode(StatusCodes.Status422UnprocessableEntity, errors);
            }
            catch (AuthenticationException ex)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new ErrorResponseDto
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
