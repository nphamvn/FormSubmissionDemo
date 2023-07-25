using FormSubmissionDemo.Data;
using FormSubmissionDemo.Entities;
using FormSubmissionDemo.Models.Shared;
using Microsoft.EntityFrameworkCore;

namespace FormSubmissionDemo.Services;
public interface ITempFileService
{
    Task<TempFile> Get(int fileId);
    Task<int> Save(TempFile file);
}
public class TempFileService(AppDbContext dbContext)  : ITempFileService
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<TempFile> Get(int fileId)
    {
        var entity = await _dbContext.TempFiles.FirstAsync(f => f.FileId == fileId);
        return new TempFile()
        {
            Content = entity.Content
        };
    }

    public async Task<int> Save(TempFile file)
    {
        await _dbContext.TempFiles.AddAsync(new TempFileEntity()
        {
            Content = file.Content
        });
        return await _dbContext.SaveChangesAsync();
    }
}
