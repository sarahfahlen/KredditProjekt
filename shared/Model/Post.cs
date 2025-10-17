namespace shared.Model;

public class Post {
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Content { get; set; } = "";
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public int Upvotes { get; set; } = 0;
    public int Downvotes { get; set; } = 0;
    public string Author { get; set; } = "";

    public List<Comment> Comments { get; set; } = new List<Comment>();
    
    public Post (){}
    public Post(string author, string title = "", string content = "", int upvotes = 0, int downvotes = 0) {
        Author = author;
        Title = title;
        Content = content;
        Upvotes = upvotes;
        Downvotes = downvotes;
        CreatedDate = DateTime.Now;
    }

    public override string ToString()
    {
        return $"Id: {Id}, Title: {Title}, Content: {Content}, Author: {Author}, Upvotes: {Upvotes}, Downvotes: {Downvotes}";
    }
}