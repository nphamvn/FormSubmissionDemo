using FormSubmissionDemo.Data;
using FormSubmissionDemo.Entities;

namespace FormSubmissionDemo.Services;
public interface IAppFileService
{
    Task<int> Save(TempFile file);
}
public class AppFileService(AppDbContext dbContext) : IAppFileService
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<int> Save(TempFile file)
    {
        await _dbContext.TempFiles.AddAsync(file);
        return await _dbContext.SaveChangesAsync();
    }
}
