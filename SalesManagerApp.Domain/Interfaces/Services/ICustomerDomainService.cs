using SalesManagerApp.Domain.Dtos.Requests;
using SalesManagerApp.Domain.Dtos.Responses;

namespace SalesManagerApp.Domain.Interfaces.Services
{
    public interface ICustomerDomainService
    {
        CustomerResponseDto? RegistrarCliente(CustomerRequestDto request);

        CustomerResponseDto? AtualizarCliente(Guid? id, CustomerResponseDto request);

        CustomerResponseDto? ExcluirCliente(Guid? id);

        CustomerResponseDto? ObterClientePorId(Guid? id);

        ICollection<CustomerResponseDto> ObterTodosClientes();
    }
}
