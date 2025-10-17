using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

using shared.Model;

namespace kreddit_app.Data;

public class ApiService
{
    private readonly HttpClient http;
    private readonly IConfiguration configuration;
    private readonly string baseAPI = "";

    public ApiService(HttpClient http, IConfiguration configuration)
    {
        this.http = http;
        this.configuration = configuration;
        this.baseAPI = configuration["base_api"];
    }

    public async Task<Post[]> GetPosts()
    {
        string url = $"{baseAPI}posts/";
        return await http.GetFromJsonAsync<Post[]>(url);
    }

    public async Task<Post> GetPost(int id)
    {
        string url = $"{baseAPI}posts/{id}/";
        return await http.GetFromJsonAsync<Post>(url);
    }
    

    public async Task<Post> UpvotePost(int id)
    {
        string url = $"{baseAPI}posts/{id}/upvote/";

        // Post JSON to API, save the HttpResponseMessage
        HttpResponseMessage msg = await http.PutAsJsonAsync(url, "");

        // Get the JSON string from the response
        string json = msg.Content.ReadAsStringAsync().Result;

        // Deserialize the JSON string to a Post object
        Post? updatedPost = JsonSerializer.Deserialize<Post>(json, new JsonSerializerOptions {
            PropertyNameCaseInsensitive = true // Ignore case when matching JSON properties to C# properties
        });

        // Return the updated post (vote increased)
        return updatedPost;
    }

    public async Task<Post> DownvotePost(int id)
    {
        string url = $"{baseAPI}posts/{id}/downvote/";
        
        HttpResponseMessage msg = await http.PutAsJsonAsync(url, "");
        
        string json = msg.Content.ReadAsStringAsync().Result;

        Post? updatedPost = JsonSerializer.Deserialize<Post>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        return updatedPost;
    }
    
    public async Task<Post> CreatePost( string title, string content, string author){
        string url = $"{baseAPI}posts";
        
        var postData = new { Title = title , Content = content, Author = author };
     
        // Post JSON to API, save the HttpResponseMessage
        HttpResponseMessage msg = await http.PostAsJsonAsync(url, postData);

        // Get the JSON string from the response
        string json = await msg.Content.ReadAsStringAsync();
        
        if (!msg.IsSuccessStatusCode)
        {
            Console.WriteLine($"🚨 Serverfejl ({msg.StatusCode}): {json}");
            throw new Exception($"Serverfejl {msg.StatusCode}: {json}");
        }

        // Deserialize the JSON string to a Comment object
        Post? newPost = JsonSerializer.Deserialize<Post>(json, new JsonSerializerOptions {
            PropertyNameCaseInsensitive = true // Ignore case when matching JSON properties to C# properties 
        });

        // Return the new comment 
        return newPost;
    }
    
    public async Task<Comment> UpvoteComment(int postid, int commentid)
    {
        string url = $"{baseAPI}posts/{postid}/comments/{commentid}/upvote";
        
        HttpResponseMessage msg = await http.PutAsJsonAsync(url, "");
        
        string json = msg.Content.ReadAsStringAsync().Result;

        Comment? updatedComment = JsonSerializer.Deserialize<Comment>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        return updatedComment;
    }
    
    
    public async Task<Comment> DownvoteComment(int postid, int commentid)
    {
        string url = $"{baseAPI}posts/{postid}/comments/{commentid}/downvote";
        
        HttpResponseMessage msg = await http.PutAsJsonAsync(url, "");
        
        string json = msg.Content.ReadAsStringAsync().Result;

        Comment? updatedComment = JsonSerializer.Deserialize<Comment>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        return updatedComment;
    }
    
    
    public async Task<Comment> CreateComment( int postId, string author, string content)
    {
        string url = $"{baseAPI}posts/{postId}/comments";
        
        var commentData = new { Author = author, Content = content };
     
        // Post JSON to API, save the HttpResponseMessage
        HttpResponseMessage msg = await http.PostAsJsonAsync(url, commentData);

        // Get the JSON string from the response
        string json = await msg.Content.ReadAsStringAsync();
        
        if (!msg.IsSuccessStatusCode)
        {
            Console.WriteLine($"🚨 Serverfejl ({msg.StatusCode}): {json}");
            throw new Exception($"Serverfejl {msg.StatusCode}: {json}");
        }

        // Deserialize the JSON string to a Comment object
        Comment? newComment = JsonSerializer.Deserialize<Comment>(json, new JsonSerializerOptions {
            PropertyNameCaseInsensitive = true // Ignore case when matching JSON properties to C# properties 
        });

        // Return the new comment 
        return newComment;
    }
}
