using SalesManagerApp.Domain.Dtos.Requests;
using SalesManagerApp.Domain.Dtos.Responses;

namespace SalesManagerApp.Domain.Interfaces.Services
{
    public interface IOrderDomainService
    {
        OrderResponseDto CriarPedido(CreateOrderRequestDto request);

        OrderResponseDto AtualizarStatusDoPedido(Guid id, UpdateOrderStatusRequestDto request);
        
        ICollection<OrderResponseDto> ListarPedidos(int pageNumber, int pageSize);

        OrderResponseDto ObterPedidoPorId(Guid id);
    }
}
