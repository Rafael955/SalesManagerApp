using FluentValidation;
using SalesManagerApp.Domain.Dtos.Requests;

namespace SalesManagerApp.Domain.Validations
{
    public class ProductValidator : AbstractValidator<ProductRequestDto>
    {
        public ProductValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("O nome do produto é obrigatório.")
                .MaximumLength(100).WithMessage("O nome do produto não pode exceder 100 caracteres.");

            RuleFor(p => p.Price)
                .GreaterThan(0).WithMessage("O preço do produto deve ser maior que zero.")
                .PrecisionScale(10, 2, false).WithMessage("O preço do produto deve ter no máximo 10 dígitos, com 2 casas decimais.");

            RuleFor(p => p.Quantity)
                .GreaterThanOrEqualTo(0).WithMessage("A quantidade do produto não pode ser negativa.");
        }
    }
}
