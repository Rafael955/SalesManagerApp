using SalesManagerApp.Domain.Dtos.Requests;
using SalesManagerApp.Domain.Dtos.Responses;
using SalesManagerApp.Domain.Interfaces.Services;

namespace SalesManagerApp.Domain.Services
{
    public class OrderDomainService : IOrderDomainService
    {
        public OrderResponseDto? CriarPedido(CreateOrderRequestDto request)
        {
            throw new NotImplementedException();
        }

        public OrderResponseDto? CancelarPedido(Guid? id)
        {
            throw new NotImplementedException();
        }

        public OrderResponseDto? AtualizarStatusDoPedido(Guid? id, UpdateOrderStatusRequestDto requestDto)
        {
            throw new NotImplementedException();
        }

        public ICollection<OrderResponseDto> ListarPedidos(int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }
    }
}
