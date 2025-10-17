using Microsoft.EntityFrameworkCore;
using shared.Model;

namespace Data
{
    public class PostsContext : DbContext
    {
        public DbSet<Post> Posts => Set<Post>();
        
        public PostsContext (DbContextOptions<PostsContext> options)
            : base(options)
        {
            // Den her er tom. Men ": base(options)" sikre at constructor
            // på DbContext super-klassen bliver kaldt.
        }
    }
}