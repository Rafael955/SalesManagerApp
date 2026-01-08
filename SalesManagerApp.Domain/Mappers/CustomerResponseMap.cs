using SalesManagerApp.Domain.Dtos.Responses;
using SalesManagerApp.Domain.Entities;
using SalesManagerApp.Domain.Helpers;

namespace SalesManagerApp.Domain.Mappers
{
    public static class CustomerResponseMap
    {
        public static CustomerResponseDto MapToResponse(this Customer customer)
        {
            return new CustomerResponseDto
            {
                Id = customer.Id,
                Name = customer.Name,
                Email = customer.Email,
                Phone = customer.Phone,
                Orders = customer.Orders?.Select(order => new OrderResponseDto
                {
                    Id = order!.Id,
                    OrderDate = order.OrderDate,
                    TotalValue = order.TotalValue,
                    OrderStatus = order.Status.GetDescription(),
                    CustomerId = order.CustomerId,
                    Customer = order.Customer == null ? null : new CustomerResponseDto
                    {
                        Id = order.Customer!.Id,
                        Name = order.Customer.Name,
                        Email = order.Customer.Email,
                        Phone = order.Customer.Phone
                    },
                    OrderItems = order.OrderItems == null ? null : order.OrderItems!.Select(oi => new OrderItemResponseDto
                    {
                        Id = oi.Id,
                        Quantity = oi.Quantity,
                        UnitPrice = oi.UnitPrice,
                        ProductName = oi.Product!.Name
                    }).ToList()
                }).ToList()
            };
        }
    }
}
