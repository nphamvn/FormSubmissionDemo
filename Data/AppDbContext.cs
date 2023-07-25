using FormSubmissionDemo.Entities;
using Microsoft.EntityFrameworkCore;

namespace FormSubmissionDemo.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<AppFileEntity> Files { get; set; }
    public DbSet<TempFileEntity> TempFiles { get; set; }
}
