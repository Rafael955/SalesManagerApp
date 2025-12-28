using FluentValidation;
using SalesManagerApp.Domain.Dtos.Requests;

namespace SalesManagerApp.Domain.Validations
{
    public class UpdateOrderStatusValidator : AbstractValidator<UpdateOrderStatusRequestDto>
    {
        public UpdateOrderStatusValidator()
        {
            RuleFor(uo => uo.OrderStatus)
                .InclusiveBetween(0, 4).WithMessage("O status do pedido deve ser um valor válido entre 0 e 4.");
        }
    }
}
