using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.Json;

using Data;
using Service;
using shared.Model;

var builder = WebApplication.CreateBuilder(args);

// Sætter CORS så API'en kan bruges fra andre domæner
var AllowSomeStuff = "_AllowSomeStuff";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: AllowSomeStuff, builder => {
        builder.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

// Tilføj DbContext factory som service.
builder.Services.AddDbContext<PostsContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("ContextSQLite")));

// Tilføj DataService så den kan bruges i endpoints
builder.Services.AddScoped<DataService>();

var app = builder.Build();

// Seed data hvis nødvendigt.
using (var scope = app.Services.CreateScope())
{
    var dataService = scope.ServiceProvider.GetRequiredService<DataService>();
    dataService.SeedData(); // Fylder data på, hvis databasen er tom. Ellers ikke.
}

app.UseHttpsRedirection();
app.UseCors(AllowSomeStuff);

// Middlware der kører før hver request. Sætter ContentType for alle responses til "JSON".
app.Use(async (context, next) =>
{
    context.Response.ContentType = "application/json; charset=utf-8";
    await next(context);
});


// DataService fås via "Dependency Injection" (DI)
app.MapGet("/", (DataService service) =>
{
    return new { message = "Hello World!" };
});


app.MapGet("/api/posts", (DataService service) =>
{
    return service.Get50Posts();
});

app.MapGet("/api/posts/{id}", (DataService service, int id) => {
    return service.GetPost(id);
});

app.MapPut("/api/posts/{id}/upvote", (DataService service, int id) =>
{
    var post = service.UpvotePost(id);
    return post is null ? Results.NotFound() : Results.Ok(post);
});

app.MapPut("/api/posts/{id}/downvote", (DataService service, int id) =>
{
    var post = service.DownvotePost(id);
    return post is null ? Results.NotFound() : Results.Ok(post);
});

app.MapPut("/api/posts/{postid}/comments/{commentid}/upvote", (DataService service, int postid, int commentid) =>
{
    var comment = service.UpvoteComment(postid, commentid);
    return comment is null ? Results.NotFound() : Results.Ok(comment);
});

app.MapPut("/api/posts/{postid}/comments/{commentid}/downvote", (DataService service, int postid, int commentid) =>
{
   var comment = service.DownvoteComment(postid, commentid);
   return comment is null ? Results.NotFound() : Results.Ok(comment);
});

app.MapPost("/api/posts", (DataService service, PostData post) =>
{
    var newPost = service.CreatePost(post.Author, post.Title, post.Content);
    return Results.Ok(newPost);
});

app.MapPost("/api/posts/{id}/comments", (DataService service, int id, CommentData comment) =>
{
    var newComment = service.CreateComment(id, comment.Author, comment.Content);
    return Results.Ok(newComment);
});

app.Run();

public record PostData(string Author, string Title, string Content);
public record CommentData(string Author, string Content);