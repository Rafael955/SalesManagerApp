using SalesManagerApp.Domain.Dtos.Requests;
using SalesManagerApp.Domain.Dtos.Responses;
using SalesManagerApp.Domain.Interfaces.Services;

namespace SalesManagerApp.Domain.Services
{
    public class CustomerDomainService : ICustomerDomainService
    {
        public CustomerResponseDto? RegistrarCliente(CustomerRequestDto request)
        {
            throw new NotImplementedException();
        }

        public CustomerResponseDto? AtualizarCliente(Guid? id, CustomerResponseDto request)
        {
            throw new NotImplementedException();
        }

        public CustomerResponseDto? ExcluirCliente(Guid? id)
        {
            throw new NotImplementedException();
        }

        public CustomerResponseDto? ObterClientePorId(Guid? id)
        {
            throw new NotImplementedException();
        }

        public ICollection<CustomerResponseDto> ObterTodosClientes()
        {
            throw new NotImplementedException();
        }
    }
}
