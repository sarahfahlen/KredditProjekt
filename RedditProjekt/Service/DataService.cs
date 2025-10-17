using Microsoft.EntityFrameworkCore;
using Data;
using shared.Model;

namespace Service;

public class DataService
{
    private PostsContext db { get; }

    public DataService(PostsContext db)
    {
        this.db = db;
    }

    /// <summary>
    /// Seeder noget nyt data i databasen hvis det er nødvendigt.
    /// </summary>
    public void SeedData()
    {
            if (db.Posts.Any()) return; // spring over hvis der allerede findes data

            var post1 = new Post("Katrine","Bedste opskrift på lasagne?", 
                "Jeg har prøvet flere versioner, men vil gerne høre jeres favorit-tips.", 
                5, 1);

            post1.Comments.Add(new Comment("Sarah",
                "Jeg plejer at lave en bechamelsauce med lidt muskat – det gør hele forskellen!", 
                3, 0));

            post1.Comments.Add(new Comment("Sissel",
                "Prøv at bruge frisk pasta i stedet for tør – det giver en bedre konsistens.", 
                2, 0));

            var post2 = new Post("Sarah", 
                "Gode tips til hjemmelavet pizza", 
                "Jeg har fundet min favoritopskrift her: https://www.valdemarsro.dk/hjemmelavet-pizza/", 
                8, 2);

            post2.Comments.Add(new Comment("Katrine",
                "Brug pizzasten i ovnen – det gør bunden sprød!", 
                4, 1));
            
            db.Posts.AddRange(post1, post2);
            db.SaveChanges();
    }
    
    public List<Post> Get50Posts()
    {
        return db.Posts
            .Include(p => p.Comments)
            .OrderByDescending(post => post.CreatedDate)
            .Take(50)
            .ToList();
    }

    public Post? GetPost(int id)
    {
        return db.Posts
            .Include(p => p.Comments)
            .FirstOrDefault(p => p.Id == id);
    }

    public Post? UpvotePost(int id)
    {
        Post postToUpdate = GetPost(id);
        
        if (postToUpdate == null)
            return null;
        
        postToUpdate.Upvotes ++;
        db.SaveChanges();
        return postToUpdate;
    }
    
    public Post? DownvotePost(int id)
    {
        Post postToUpdate = GetPost(id);
        
        if (postToUpdate == null)
            return null;
        
        postToUpdate.Downvotes ++;
        db.SaveChanges();
        return postToUpdate;
    }

    public Comment? UpvoteComment(int postId, int commentId)
    {
        Post post = GetPost(postId);
        if (post == null)
            return null;
        
        Comment commentToUpdate = post.Comments.FirstOrDefault(c => c.Id == commentId);
        if (commentToUpdate == null)
            return null;
        
        commentToUpdate.Upvotes ++;
        db.SaveChanges();
        return commentToUpdate;
    }
    
    public Comment? DownvoteComment(int postId, int commentId)
    {
        Post post = GetPost(postId);
        if (post == null)
            return null;
        
        Comment commentToUpdate = post.Comments.FirstOrDefault(c => c.Id == commentId);
        if (commentToUpdate == null)
            return null;
        
        commentToUpdate.Downvotes ++;
        db.SaveChanges();
        return commentToUpdate;
    }

    public Post CreatePost(string author, string title, string? text)
    {
        var newpost = new Post(author, title, text);
        db.Posts.Add(newpost);
        db.SaveChanges();
        return newpost;
    }
    
    public Comment CreateComment(int postId, string author, string text)
    {
        var post = db.Posts
            .Include(p => p.Comments)
            .FirstOrDefault(p => p.Id == postId);

        if (post == null)
        {
            throw new Exception("Post not found");
        }
        
        var newcomment = new Comment(author, text);

        post.Comments.Add(newcomment);
        db.SaveChanges();

        return newcomment;
    }
    
    }