public class PodcastResponse
{
    public PodcastResponse(Guid Id, DateTime DateTime, string Image, string Content, string Title)
    {
        this.Id = Id;
        this.DateTime = DateTime;
        this.Image = Image;
        this.Content = Content;
        this.Title = Title;
    }
    public Guid Id { get; set; }
    public DateTime DateTime { get; set; }
    public string Image { get; set; }
    public string Content { get; set; }
    public string Title { get; set; }
}
