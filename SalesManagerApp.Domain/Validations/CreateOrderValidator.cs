using FluentValidation;
using SalesManagerApp.Domain.Dtos.Requests;

namespace SalesManagerApp.Domain.Validations
{
    public class CreateOrderValidator : AbstractValidator<CreateOrderRequestDto>
    {
        public CreateOrderValidator()
        {
            RuleFor(o => o.CustomerId)
                .NotEmpty().WithMessage("O Id do cliente é obrigatório.");

            RuleFor(o => o.OrderItems)
                .Must(oi => oi != null && oi.Count > 0).WithMessage("O pedido deve conter pelo menos um item.");
        }
    }
}
