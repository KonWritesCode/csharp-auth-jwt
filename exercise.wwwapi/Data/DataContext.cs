using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using workshop.webapi.DataModels;

namespace workshop.webapi.Data
{
    public class DataContext : IdentityUserContext<ApplicationUser>
    {
        public DataContext(DbContextOptions<DataContext> options)
       : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //further information
            //https://learn.microsoft.com/en-us/aspnet/core/security/authentication/customize-identity-model?view=aspnetcore-8.0


            modelBuilder.Entity<Blog>().HasData(new Blog { Id = 1, BlogTitle = "Mini's Cooking recipes" });
            modelBuilder.Entity<Blog>().HasData(new Blog { Id = 2,  BlogTitle = "Cool cars blog" });
            modelBuilder.Entity<Blog>().HasData(new Blog { Id = 3,  BlogTitle = "Best movies ever" });
            modelBuilder.Entity<Blog>().HasData(new Blog { Id = 4,  BlogTitle = "Fantastic News: Official" });
            modelBuilder.Entity<Blog>().HasData(new Blog { Id = 5,  BlogTitle = "Music Corner" });
            modelBuilder.Entity<Blog>().HasData(new Blog { Id = 6,  BlogTitle = "Tomato Farm Inc." });
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Blog> Cars { get; set; }
    }
}
