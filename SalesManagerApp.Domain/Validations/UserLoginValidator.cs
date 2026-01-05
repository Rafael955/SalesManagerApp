using FluentValidation;
using SalesManagerApp.Domain.Dtos.Requests;

namespace SalesManagerApp.Domain.Validations
{
    public class UserLoginValidator : AbstractValidator<UserLoginRequestDto>
    {
        public UserLoginValidator()
        {
            RuleFor(ul => ul.Email)
                .NotEmpty()
                    .WithMessage("O email é obrigatório.")
                .EmailAddress()
                    .WithMessage("O email deve ser um endereço de email válido.");

            RuleFor(ul => ul.Password)
                .NotEmpty()
                    .WithMessage("A senha é obrigatória.")
                .MinimumLength(8)
                    .WithMessage("A senha deve ter no mínimo 8 caracteres.")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$")
                    .WithMessage("A senha deve conter pelo menos uma letra maiúscula, uma letra minúscula, um número e um caractere especial.");
        }
    }
}
