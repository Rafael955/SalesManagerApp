using FluentValidation;
using SalesManagerApp.Domain.Dtos.Requests;
using SalesManagerApp.Domain.Dtos.Responses;
using SalesManagerApp.Domain.Entities;
using SalesManagerApp.Domain.Enums;
using SalesManagerApp.Domain.Interfaces.Repositories;
using SalesManagerApp.Domain.Interfaces.Services;
using SalesManagerApp.Domain.Mappers;
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
                throw new ApplicationException("O pedido com este Id não existe!");

            return orderCreated.MapToResponseDto();
        }

        public OrderResponseDto AtualizarStatusDoPedido(Guid id, UpdateOrderStatusRequestDto request)
        {
            var validation = new UpdateOrderStatusValidator().Validate(request);

            if(!validation.IsValid)
                throw new ValidationException(validation.Errors);

            var order = orderRepository.GetById(id);

            if (order == null)
                throw new ApplicationException("O pedido com este Id não existe!");

            if (order.Status == OrderStatus.Completed)
                throw new ApplicationException("Não será possível atualizar os status do pedido pois este pedido já foi concluido!");

            if (order.Status == OrderStatus.Cancelled)
                throw new ApplicationException("Não será possível atualizar os status do pedido pois este pedido foi cancelado!");

            order.Status = request.Status;

            orderRepository.Update(order);

            return order.MapToResponseDto();
        }

        public ICollection<OrderResponseDto> ListarPedidos(int pageNumber, int pageSize)
        {
            return orderRepository.GetPaginatedList(pageNumber, pageSize).Select(order => order.MapToResponseDto()).ToList();
        }

        public OrderResponseDto ObterPedidoPorId(Guid id)
        {
            var order = orderRepository.GetById(id);

            if (order == null)
                throw new ApplicationException("O pedido com este Id não existe!");

            return order.MapToResponseDto();
        }
    }
}
