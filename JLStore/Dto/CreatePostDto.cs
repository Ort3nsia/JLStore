namespace JLStore.Dto;

public class CreatePostDto
{
    public string Title { get; set; }
    public string Content { get; set; }
    public bool PublishNow { get; set; } = true;
    public DateTimeOffset? PublishedAtUtc { get; set; }

    public CreatePostDto(string title, string content, bool publishNow = true, DateTimeOffset? publishedAtUtc = null)
    {
        Title = title;
        Content = content;
        PublishNow = publishNow;
        PublishedAtUtc = publishedAtUtc;
    }
}
