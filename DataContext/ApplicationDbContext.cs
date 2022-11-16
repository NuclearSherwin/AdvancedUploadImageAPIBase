using FileUploadBase.Models;
using Microsoft.EntityFrameworkCore;

namespace FileUploadBase.DataContext;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
        : base(options)
    {
        
    }
    
    public DbSet<PostModel> PostModels { get; set; }
    public DbSet<Post> Posts { get; set; }

}