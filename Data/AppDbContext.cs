using FormSubmissionDemo.Entities;
using Microsoft.EntityFrameworkCore;

namespace FormSubmissionDemo.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<AppFile> Files { get; set; }
    public DbSet<TempFile> TempFiles { get; set; }
}
