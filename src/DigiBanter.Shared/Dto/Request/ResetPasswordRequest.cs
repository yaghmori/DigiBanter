using FluentValidation;

namespace DigiBanter.Shared.Dto.Request
{
    public class ResetPasswordRequest : PasswordRequest

    {
        public string Token { get; set; } = default!;


    }

    public class ResetPasswordValidator : AbstractValidator<ResetPasswordRequest>
    {

        public ResetPasswordValidator()
        {
            Include(new PasswordValidator());
            RuleFor(x => x.Token).NotEmpty().WithMessage("OTP token is required.")
            .Length(6).WithMessage("OTP token must be 6 digits.")
            .Matches(@"^\d{6}$").WithMessage("OTP token must be a 6-digit number.");

        }

    }

}
