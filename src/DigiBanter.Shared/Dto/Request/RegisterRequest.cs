using FluentValidation;

namespace DigiBanter.Shared.Dto.Request
{
    public class RegisterRequest : PasswordRequest
    {

        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        //==========computed
        public string NormalizedEmail => Email.Normalize().ToUpper();
        public string UserName => Email;
        public string NormalizedUserName => UserName.Normalize().ToUpper();
        public string SecurityStamp => Guid.NewGuid().ToString();
        public string ConcurrencyStamp => Guid.NewGuid().ToString();

    }
    public class RegisterValidator : AbstractValidator<RegisterRequest>
    {

        public RegisterValidator()
        {
            Include(new PasswordValidator());

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .Matches(@"^[\w\.-]+@[a-zA-Z\d\.-]+\.[a-zA-Z]{2,}$")
                .WithMessage("Please enter a valid email address.");


            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"^\(\d{3}\) \d{3}-\d{4}$")
                .WithMessage("Phone number must be in the format: (123) 456-7890");

        }

    }

}
