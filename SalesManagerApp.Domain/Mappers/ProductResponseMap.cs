using SalesManagerApp.Domain.Dtos.Responses;
using SalesManagerApp.Domain.Entities;

namespace SalesManagerApp.Domain.Mappers
{
    public static class ProductResponseMap
    {
        public static ProductResponseDto MapToResponseDto(this Product product)
        {
            return new ProductResponseDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Quantity = product.Quantity
            };
        }
    }
}
