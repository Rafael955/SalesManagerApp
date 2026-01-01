using FluentValidation;
using SalesManagerApp.Domain.Dtos.Requests;
using SalesManagerApp.Domain.Dtos.Responses;
using SalesManagerApp.Domain.Entities;
using SalesManagerApp.Domain.Interfaces.Repositories;
using SalesManagerApp.Domain.Interfaces.Services;
using SalesManagerApp.Domain.Validations;

namespace SalesManagerApp.Domain.Services
{
    public class ProductDomainService(IProductRepository productRepository) : IProductDomainService
    {
        public ProductResponseDto? CriarProduto(ProductRequestDto request)
        {
            var validation = new ProductValidator().Validate(request);

            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);

            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Price = request.Price,
                Quantity = request.Quantity
            };

            productRepository.Add(product);

            return new ProductResponseDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Quantity = product.Quantity
            };
        }

        public ProductResponseDto? AtualizarProduto(Guid? id, ProductRequestDto request)
        {
            var product = productRepository.GetById(id.Value);

            if (product == null)
                throw new ApplicationException("O produto com este Id não existe!");

            var validation = new ProductValidator().Validate(request);

            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);

            product.Name = request.Name;
            product.Price = request.Price;
            product.Quantity = request.Quantity;

            productRepository.Update(product);

            return new ProductResponseDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Quantity = product.Quantity
            };
        }

        public void ExcluirProduto(Guid? id)
        {
            var product = productRepository.GetById(id.Value);

            if (product == null)
                throw new ApplicationException("O produto com este Id não existe!");

            productRepository.Delete(product);
        }

        public ProductResponseDto? ObterProdutoPorId(Guid? id)
        {
            var product = productRepository.GetById(id.Value);

            if (product == null)
                throw new ApplicationException("O produto com este Id não existe!");

            return new ProductResponseDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Quantity = product.Quantity
            };
        }

        public ICollection<ProductResponseDto> ObterTodosProdutos()
        {
            var products = productRepository.GetAll();

            List<ProductResponseDto> listProductsDto = new List<ProductResponseDto>();

            foreach (var product in products)
            {
                listProductsDto.Add(new ProductResponseDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Quantity = product.Quantity
                });
            }

            return listProductsDto;
        }
    }
}
