using SalesManagerApp.Domain.Dtos.Requests;
using SalesManagerApp.Domain.Dtos.Responses;

namespace SalesManagerApp.Domain.Interfaces.Services
{
    public interface IProductDomainService
    {
        ProductResponseDto? CriarProduto(ProductRequestDto request);

        ProductResponseDto? AtualizarProduto(Guid? id, ProductRequestDto request);

        void ExcluirProduto(Guid? id);

        ProductResponseDto? ObterProdutoPorId(Guid? id);

        ICollection<ProductResponseDto> ObterTodosProdutos();
    }
}
