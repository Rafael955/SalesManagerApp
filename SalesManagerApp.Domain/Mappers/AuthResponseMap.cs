using SalesManagerApp.Domain.Dtos.Responses;
using SalesManagerApp.Domain.Entities;
using SalesManagerApp.Domain.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesManagerApp.Domain.Mappers
{
    public static class AuthResponseMap
    {
        public static UserLoginResponseDto MapToResponseDto(this User user)
        {
            return new UserLoginResponseDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role.GetDescription(),
                AccessToken = JwtTokenHelper.GenerateToken(user.Email!, user.Role.GetDescription())
            };
        }
    }
}
