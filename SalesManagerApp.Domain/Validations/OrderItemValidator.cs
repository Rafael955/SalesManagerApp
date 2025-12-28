using FluentValidation;
using SalesManagerApp.Domain.Entities;

namespace SalesManagerApp.Domain.Validations
{
    public class OrderItemValidator : AbstractValidator<OrderItem>
    {
        public OrderItemValidator()
        {
            RuleFor(oi => oi.Quantity)
                .GreaterThan(0).WithMessage("A quantidade do item do pedido deve ser maior que zero.");

            RuleFor(oi => oi.UnitPrice)
                //.GreaterThan(0).WithMessage("O preço unitário do item do pedido deve ser maior que zero.") Obs: Pode haver casos em que o preço unitário seja zero, como em promoções ou brindes.
                .PrecisionScale(10, 2, false).WithMessage("O preço unitário do item do pedido deve ter no máximo 10 dígitos, com 2 casas decimais.");

            RuleFor(oi => oi.OrderId)
                .NotEmpty().WithMessage("O Id do pedido é obrigatório.");
        }
    }
}
