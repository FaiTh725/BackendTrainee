using FluentValidation;
using Trainee.Models.Client;

namespace Trainee.Validators
{
    public class ClientValidator : AbstractValidator<CreateClient>
    {
        public ClientValidator()
        {
            RuleFor(c => c.Email)
                .MaximumLength(20).WithMessage("max lenght email 20 symbols")
                .EmailAddress().WithMessage("invalid email signature");
            
            RuleFor(c => c.Name)
                .MaximumLength(20).WithMessage("max lengh name 20 symbols");

            RuleFor(c => c.Phone)
                .MaximumLength(15).WithMessage("max lengh phone 15 symbols")
                .Matches(@"^\d+$").WithMessage("phone must containe oncly number");
        }
    }
}
