using FluentValidation;
using SalesManagerApp.Domain.Dtos.Requests;
using SalesManagerApp.Domain.Dtos.Responses;
using SalesManagerApp.Domain.Entities;
using SalesManagerApp.Domain.Enums;
using SalesManagerApp.Domain.Helpers;
using SalesManagerApp.Domain.Interfaces.Repositories;
using SalesManagerApp.Domain.Interfaces.Services;
using SalesManagerApp.Domain.Validations;

namespace SalesManagerApp.Domain.Services
{
    public class OrderDomainService(IOrderRepository orderRepository, IOrderItemRepository orderItemRepository, IProductRepository productRepository) : IOrderDomainService
    {
        public OrderResponseDto CriarPedido(CreateOrderRequestDto request)
        {
            var validation = new CreateOrderValidator().Validate(request);

            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);

            foreach (var orderItem in request.OrderItems!)
            {
                validation = new CreateOrderItemValidator().Validate(orderItem);

                if (!validation.IsValid)
                    throw new ValidationException(validation.Errors);
            }

            var order = new Order
            {
                Id = Guid.NewGuid(),
                OrderDate = DateTime.Now,
                CustomerId = request.CustomerId,
                Status = OrderStatus.Pending
            };

            foreach (var orderItem in request.OrderItems)
            {
                var orderProduct = productRepository.GetById(orderItem.ProductId!.Value);

                order.TotalValue += orderItem.Quantity * orderProduct!.Price;
            }

            orderRepository.Add(order);

            foreach (var orderItem in request.OrderItems)
            {
                var orderProduct = productRepository.GetById(orderItem.ProductId!.Value);

                orderItemRepository.Add(new OrderItem
                {
                    Id = Guid.NewGuid(),
                    OrderId = order.Id,
                    ProductId = orderItem.ProductId!.Value,
                    Quantity = orderItem.Quantity,
                    UnitPrice = orderProduct!.Price
                });
            }
            
            var orderCreated = orderRepository.GetById(order.Id);

            if (orderCreated == null)
                throw new ApplicationException("Houve um erro ao tentar criar o pedido.");

            return new OrderResponseDto
            {
                Id = order.Id,
                OrderDate = orderCreated.OrderDate,
                TotalValue = orderCreated.TotalValue,
                OrderStatus = orderCreated.Status.GetDescription(),
                CustomerId = order.CustomerId,
                Customer = new CustomerResponseDto
                {
                    Id = orderCreated.Customer!.Id,
                    Name = orderCreated.Customer.Name,
                    Email = orderCreated.Customer.Email,
                    Phone = orderCreated.Customer.Phone
                },
                OrderItems = orderCreated.OrderItems!.Select(oi => new OrderItemResponseDto
                {
                    Id = oi.Id,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice,
                    ProductName = oi.Product!.Name
                }).ToList()
            };
        }

        public OrderResponseDto CancelarPedido(Guid? id)
        {
            var order = orderRepository.GetById(id.Value);

            if(order == null)
                throw new ApplicationException("O pedido com este Id não existe!");

            if(order.Status == OrderStatus.Completed)
                throw new ApplicationException("Não será possível cancelar pois o pedido já foi concluido!");

            if(order.Status == OrderStatus.Cancelled)
                throw new ApplicationException("O pedido já está cancelado!");
            
            order.Status = OrderStatus.Cancelled;

            orderRepository.Update(order);

            return new OrderResponseDto
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                TotalValue = order.TotalValue,
                OrderStatus = order.Status.GetDescription(),
                CustomerId = order.CustomerId,
                Customer = new CustomerResponseDto
                {
                    Id = order.Customer!.Id,
                    Name = order.Customer.Name,
                    Email = order.Customer.Email,
                    Phone = order.Customer.Phone
                },
                OrderItems = order.OrderItems!.Select(oi => new OrderItemResponseDto
                {
                    Id = oi.Id,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice,
                    ProductName = oi.Product!.Name
                }).ToList()
            };
        }

        public OrderResponseDto AtualizarStatusDoPedido(Guid? id, UpdateOrderStatusRequestDto request)
        {
            var validation = new UpdateOrderStatusValidator().Validate(request);

            if(!validation.IsValid)
                throw new ValidationException(validation.Errors);

            var order = orderRepository.GetById(id.Value);

            if (order == null)
                throw new ApplicationException("O pedido com este Id não existe!");

            if (order.Status == OrderStatus.Completed)
                throw new ApplicationException("Não será possível atualizar os status do pedido pois este pedido já foi concluido!");

            if (order.Status == OrderStatus.Cancelled)
                throw new ApplicationException("Não será possível atualizar os status do pedido pois este pedido foi ccancelado!");

            order.Status = (OrderStatus)request.OrderStatus;

            orderRepository.Update(order);

            return new OrderResponseDto
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                TotalValue = order.TotalValue,
                OrderStatus = order.Status.GetDescription(),
                CustomerId = order.CustomerId,
                Customer = new CustomerResponseDto
                {
                    Id = order.Customer!.Id,
                    Name = order.Customer.Name,
                    Email = order.Customer.Email,
                    Phone = order.Customer.Phone
                },
                OrderItems = order.OrderItems!.Select(oi => new OrderItemResponseDto
                {
                    Id = oi.Id,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice,
                    ProductName = oi.Product!.Name
                }).ToList()
            };
        }

        public ICollection<OrderResponseDto> ListarPedidos(int pageNumber, int pageSize)
        {
            return orderRepository.GetPaginatedList(pageNumber, pageSize).Select(order => new OrderResponseDto
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                TotalValue = order.TotalValue,
                OrderStatus = order.Status.GetDescription(),
                CustomerId = order.CustomerId
            }).ToList();
        }
    }
}
