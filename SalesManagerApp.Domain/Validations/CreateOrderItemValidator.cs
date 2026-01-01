using FluentValidation;
using SalesManagerApp.Domain.Dtos.Requests;

namespace SalesManagerApp.Domain.Validations
{
    public class CreateOrderItemValidator : AbstractValidator<CreateOrderItemRequestDto>
    {
        public CreateOrderItemValidator()
        {
            RuleFor(oi => oi.Quantity)
                .GreaterThan(0).WithMessage("A quantidade do item do pedido deve ser maior que zero.");

            RuleFor(oi => oi.ProductId)
                .NotEmpty().WithMessage("O Id do produto é obrigatório.");
        }
    }
}
