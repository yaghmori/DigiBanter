using FluentValidation;

namespace DigiBanter.Shared.Dto.Request
{
    public class PasswordRequest
    {
        public string Password { get; set; } = default!;
        public string PasswordConfirmation { get; set; } = default!;
    }

    public class PasswordValidator : AbstractValidator<PasswordRequest>
    {

        public PasswordValidator()
        {
            RuleFor(x => x.Password).NotEmpty().WithMessage("Please specify password");
            RuleFor(x => x.Password).Length(8, 24).WithMessage("Password must be at least 8 and maximum 24 characters long");

            RuleFor(x => x.PasswordConfirmation).NotEmpty().WithMessage("Please confirm your password");
            RuleFor(x => x.PasswordConfirmation).Length(8, 24).WithMessage("Password must be at least 8 and maximum 24 characters long");
            //RuleFor(x => x.PasswordConfirmation).(x => x.Password).WithMessage("Passwords do not match");
            RuleFor(x => x.PasswordConfirmation).Must((x, PasswordConfirmation) => PasswordConfirmation == x.Password).WithMessage("Passwords do not match");

        }

    }
}
