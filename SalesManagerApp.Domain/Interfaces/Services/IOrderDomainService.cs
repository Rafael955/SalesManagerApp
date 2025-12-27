using SalesManagerApp.Domain.Dtos.Requests;
using SalesManagerApp.Domain.Dtos.Responses;

namespace SalesManagerApp.Domain.Interfaces.Services
{
    public interface IOrderDomainService
    {
        OrderResponseDto? CriarPedido(CreateOrderRequestDto request);

        OrderResponseDto? CancelarPedido(Guid? id);

        OrderResponseDto? AtualizarStatusDoPedido(Guid? id, UpdateOrderStatusRequestDto requestDto);
        
        ICollection<OrderResponseDto> ListarPedidos(int pageNumber, int pageSize);

    }
}
