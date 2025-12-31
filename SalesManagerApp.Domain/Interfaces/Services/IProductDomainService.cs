using SalesManagerApp.Domain.Dtos.Requests;

namespace SalesManagerApp.Domain.Interfaces.Services
{
    public interface IProductDomainService
    {
        ProductRequestDto? CriarProduto(ProductRequestDto request);

        ProductRequestDto? AtualizarProduto(Guid? id, ProductRequestDto request);

        ProductRequestDto? ExcluirProduto(Guid? id);

        ProductRequestDto? ObterProdutoPorId(Guid? id);

        ICollection<ProductRequestDto> ObterTodosProdutos();
    }
}
