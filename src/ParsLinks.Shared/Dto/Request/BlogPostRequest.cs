using FluentValidation;
using Newtonsoft.Json;
using ParsLinks.Shared.Extensions;

namespace ParsLinks.Shared.Dto.Request;
public class BlogPostRequest
{
    public int? Id { get; set; }
    public Guid? AuthorId { get; set; }
    public int? CategoryId { get; set; }
    public int Status { get; set; } = 1;
    public DateTime? PublishedAt { get; set; }
    public List<BlogPostTranslationRequest> Translations { get; set; } = new();



}
public class BlogPostRequestValidator : AbstractValidator<BlogPostRequest>
{
    public BlogPostRequestValidator()
    {
        //RuleFor(x => x.Image).NotEmpty()
        //    .WithMessage("The cover image is required. Please provide an image URL or path.");

        RuleFor(x => x.Status).NotNull()
            .WithMessage("The status field is required. Please select a valid status.");
        RuleFor(x => x.Translations)
            .Must(translations => translations != null && translations.Any())
            .WithMessage("At least one translation is required. Please add a translation.");

        RuleForEach(x => x.Translations).SetValidator(new BlogPostTranslationRequestValidator())
            .WithMessage("One or more translations are invalid. Please ensure all translations meet the required criteria.");


    }
}

public class BlogPostTranslationRequest
{
    [JsonIgnore]
    public Guid Key { get; set; } = Guid.NewGuid();

    public int? PostId { get; set; }
    public int? CategoryId { get; set; }
    public int LanguageId { get; set; }
    public string Title { get; set; } = default!;
    public string Slug { get; set; } = default!;
    public string Content { get; set; } = default!;

}
public class BlogPostTranslationRequestValidator : AbstractValidator<BlogPostTranslationRequest>
{
    public BlogPostTranslationRequestValidator()
    {
        //RuleFor(x => x.PostId).NotEmpty().WithMessage("Cover Image should not be empty");
        RuleFor(x => x.LanguageId).NotEmpty()
            .WithMessage("The language ID is required. Please provide a valid language identifier.");

        RuleFor(x => x.Title).NotEmpty()
            .WithMessage("The title is required. Please provide a title for the translation.");

        RuleFor(x => x.Slug).NotEmpty()
            .WithMessage("The slug is required. Please provide a slug for the translation.")
            .Custom((slug, context) =>
            {
                var request = (BlogPostTranslationRequest)context.InstanceToValidate;
                if (string.IsNullOrEmpty(slug) && !string.IsNullOrEmpty(request.Title))
                {
                    request.Slug = StringExtensionsHelpers.GenerateSlug(request.Title);
                }
            });

        //RuleFor(x => x.Content).NotEmpty()
        //    .WithMessage("The content is required. Please provide content for the translation.");

        //RuleFor(x => x.PublishedAt).NotEmpty().WithMessage("PublishedAt filed is required");
    }

}

