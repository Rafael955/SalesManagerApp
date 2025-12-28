using FluentValidation;
using SalesManagerApp.Domain.Dtos.Requests;

namespace SalesManagerApp.Domain.Validations
{
    public class AlterOrderValidator : AbstractValidator<AlterOrderRequestDto>
    {
        public AlterOrderValidator()
        {
            RuleFor(o => o.OrderItems)
                .NotEmpty().WithMessage("O pedido deve conter pelo menos um item.")
                .Must(oi => oi != null && oi.Count > 0).WithMessage("O pedido deve conter pelo menos um item.");
        }
    }
}
