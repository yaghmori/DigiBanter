using FluentValidation;

namespace ParsLinks.Shared.Dto.Response;

public class CategoryRequest
{
    public int? Id { get; set; }
    public List<CategoryTranslationRequest> Translations { get; set; } = new();
}

public class CategoryRequestValidator : AbstractValidator<CategoryRequest>
{
    public CategoryRequestValidator()
    {
        //RuleFor(x => x.Image).NotEmpty()
        //    .WithMessage("The cover image is required. Please provide an image URL or path.");

        RuleFor(x => x.Translations)
            .Must(translations => translations != null && translations.Any())
            .WithMessage("At least one translation is required. Please add a translation.");

        RuleForEach(x => x.Translations).SetValidator(new CategoryTranslationRequestValidator())
            .WithMessage("One or more translations are invalid. Please ensure all translations meet the required criteria.");


    }
}

public class CategoryTranslationRequest
{
    public int? Id { get; set; }
    public int CategoryId { get; set; }
    public int LanguageId { get; set; }
    public string Name { get; set; }
    public string Slug { get; set; }
    public string Description { get; set; }



}

public class CategoryTranslationRequestValidator : AbstractValidator<CategoryTranslationRequest>
{
    public CategoryTranslationRequestValidator()
    {
        //RuleFor(x => x.PostId).NotEmpty().WithMessage("Cover Image should not be empty");
        RuleFor(x => x.LanguageId).NotEmpty()
            .WithMessage("The language ID is required. Please provide a valid language identifier.");

        RuleFor(x => x.Name).NotEmpty()
            .WithMessage("The name is required. Please provide a title for the translation.");

        RuleFor(x => x.Slug).NotEmpty()
            .WithMessage("The slug is required. Please provide a slug for the translation.");
    }

}
