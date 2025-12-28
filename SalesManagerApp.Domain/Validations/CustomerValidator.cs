using FluentValidation;
using SalesManagerApp.Domain.Dtos.Requests;

namespace SalesManagerApp.Domain.Validations
{
    public class CustomerValidator : AbstractValidator<CustomerRequestDto>
    {
        public CustomerValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("O nome do cliente é obrigatório.")
                .MaximumLength(150).WithMessage("O nome do cliente não pode exceder 100 caracteres.");

            RuleFor(c => c.Email)
                .NotEmpty().WithMessage("O email do cliente é obrigatório.")
                .EmailAddress().WithMessage("O email do cliente deve ser um endereço de email válido.")
                .MaximumLength(100).WithMessage("O email do cliente não pode exceder 100 caracteres.");

            RuleFor(c => c.Phone)
                .NotEmpty().WithMessage("O telefone do cliente é obrigatório.")
                .MaximumLength(15).WithMessage("O telefone do cliente não pode exceder 15 caracteres.");
        }
    }
}
