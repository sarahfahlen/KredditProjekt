namespace shared.Model;

public class Comment
{
    public int Id { get; set; }
    public string Content { get; set; } = "";
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public string Author { get; set; } = "";
    public int Upvotes { get; set; } = 0;
    public int Downvotes { get; set; } = 0;

    public Comment(){}
    public Comment(string author, string content = "", int upvotes = 0, int downvotes = 0, DateTime? createdDate = null)
    {
        Author = author;
        Content = content;
        Upvotes = upvotes;
        Downvotes = downvotes;
        CreatedDate = createdDate ?? DateTime.Now;
    }
    
    public override string ToString()
    {
        return $"Id: {Id}, Author: {Author}, Content: {Content}, Upvotes: {Upvotes}, Downvotes: {Downvotes}";
    }
}
