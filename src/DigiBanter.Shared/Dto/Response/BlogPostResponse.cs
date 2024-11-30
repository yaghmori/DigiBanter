
namespace DigiBanter.Shared.Dto.Response;
public class BlogPostResponse
{

    public int Id { get; set; }
    public string Author { get; set; } = default!;
    public string Image  { get; set; } = default!;
    public string Title  { get; set; } = default!;
    public string Content  { get; set; } = default!;
    public string Slug  { get; set; } = default!;
    public string Language  { get; set; } = default!;
    public DateTime PublishedAt { get; set; }



}

