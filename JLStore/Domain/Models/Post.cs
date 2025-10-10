namespace JLStore.Domain.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Content { get; set; } = "";

        public bool Published { get; set; }
        public DateTimeOffset? PublishedAt { get; set; }   // usa UTC
        public bool IsDeleted { get; set; }
    }
}
