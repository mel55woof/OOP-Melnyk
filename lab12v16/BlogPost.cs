namespace lab12v16
{
    public class BlogPost : IEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int AuthorId { get; set; }

        public BlogPost(int id, string title, string content, int authorId)
        {
            Id = id;
            Title = title;
            Content = content;
            AuthorId = authorId;
        }
    }
}
