using SalesManagerApp.Domain.Dtos.Requests;
using SalesManagerApp.Domain.Interfaces.Services;

namespace SalesManagerApp.Domain.Services
{
    public class ProductDomainService : IProductDomainService
    {
        public ProductRequestDto? CriarProduto(ProductRequestDto request)
        {
            throw new NotImplementedException();
        }

        public ProductRequestDto? AtualizarProduto(Guid? id, ProductRequestDto request)
        {
            throw new NotImplementedException();
        }

        public ProductRequestDto? ExcluirProduto(Guid? id)
        {
            throw new NotImplementedException();
        }

        public ProductRequestDto? ObterProdutoPorId(Guid? id)
        {
            throw new NotImplementedException();
        }

        public ICollection<ProductRequestDto> ObterTodosProdutos()
        {
            throw new NotImplementedException();
        }
    }
}
