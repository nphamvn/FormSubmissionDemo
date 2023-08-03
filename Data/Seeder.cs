using FormSubmissionDemo.Entities;

namespace FormSubmissionDemo.Data;

public static class Seeder
{
    public static void SeedData(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetService<AppDbContext>();

        var defaultUserImage = new AppFileEntity() {
            FileId = 1,
            Content = File.ReadAllBytes("Images/user.png")
        };
        dbContext.Files.Add(defaultUserImage);
        dbContext.Users.Add(new()
        {
            UserId = 1,
            Username = "Nam Pham",
            ProfileFileId = defaultUserImage.FileId
        });
        dbContext.SaveChanges();
    }
}
