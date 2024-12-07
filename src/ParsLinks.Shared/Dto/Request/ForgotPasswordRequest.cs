using FluentValidation;

namespace ParsLinks.Shared.Dto.Request
{
    public class ForgotPasswordRequest

    {
        public string Email { get; set; } = string.Empty;

    }
    public class ForgotPasswordValidator : AbstractValidator<ForgotPasswordRequest>
    {

        public ForgotPasswordValidator()
        {

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .Matches(@"^[\w\.-]+@[a-zA-Z\d\.-]+\.[a-zA-Z]{2,}$")
                .WithMessage("Please enter a valid email address.");
        }

    }
}
