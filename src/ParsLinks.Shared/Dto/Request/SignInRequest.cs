using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace ParsLinks.Shared.Dto.Request
{
    public class SignInByUserNameRequest
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required, DataType(DataType.Password)]
        [StringLength(30, ErrorMessage = "Password must be at least 8 characters long.", MinimumLength = 8)]
        public string Password { get; set; } = string.Empty;

        [RegularExpression("([0-9]+)")]
        public bool IsPersistent { get; set; }
        public string NormalizedUsername => Username.Normalize().ToUpper();

    }
    public class SignInByPhoneNumberRequest
    {
        [Required]
        [RegularExpression("([0-9]+)")]
        public string PhoneNumber { get; set; } = string.Empty;
        public bool IsPersistent { get; set; }
    }
    public class LoginByEmailRequest
    {
        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public bool IsPersistent { get; set; }
    }
    public class LoginByEmailValidator : AbstractValidator<LoginByEmailRequest>
    {

        public LoginByEmailValidator()
        {

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .Matches(@"^[\w\.-]+@[a-zA-Z\d\.-]+\.[a-zA-Z]{2,}$")
                .WithMessage("Please enter a valid email address.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .Length(8, 24).WithMessage("Password must be at least 8 and maximum 24 characters long");
        }
    }


    public class NewPhoneNumberRequest
    {
        public string PhoneNumber { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

    }


    public class NewEmailRequest
    {
        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

    }
    public class NewEmailRequestValidator : AbstractValidator<NewEmailRequest>
    {

        public NewEmailRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .Matches(@"^[\w\.-]+@[a-zA-Z\d\.-]+\.[a-zA-Z]{2,}$")
                .WithMessage("Please enter a valid email address.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .Length(8, 24).WithMessage("Password must be at least 8 and maximum 24 characters long");
        }
    }

    public class NewPhoneNumberRequestValidator : AbstractValidator<NewPhoneNumberRequest>
    {

        public NewPhoneNumberRequestValidator()
        {

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"^\(\d{3}\) \d{3}-\d{4}$")
                .WithMessage("Phone number must be in the format: (123) 456-7890");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .Length(8, 24).WithMessage("Password must be at least 8 and maximum 24 characters long");
        }
    }

}
