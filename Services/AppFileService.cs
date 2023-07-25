using FormSubmissionDemo.Data;
using FormSubmissionDemo.Entities;
using FormSubmissionDemo.Models.Shared;
using Microsoft.EntityFrameworkCore;

namespace FormSubmissionDemo.Services;
public interface IAppFileService
{
    Task<int> Save(AppFile file);
    Task<int> CopyFromTempFile(int tempFileId);
    Task<AppFile> Get(int fileId);
}
public class AppFileService(AppDbContext dbContext) : IAppFileService
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<int> Save(AppFile file)
    {
        await _dbContext.Files.AddAsync(new AppFileEntity()
        {
            Content = file.Content
        });
        return await _dbContext.SaveChangesAsync();
    }

    public async Task<int> CopyFromTempFile(int tempFileId)
    {
        var tempFile = await _dbContext.TempFiles.FirstAsync(f => f.FileId == tempFileId);
        var file = new AppFileEntity()
        {
            Content = tempFile.Content
        };
        _dbContext.Files.Add(file);
        return file.FileId;
    }

    public async Task<AppFile> Get(int fileId)
    {
        var fileEntiy = await _dbContext.Files.FirstAsync(f => f.FileId == fileId);
        return new AppFile()
        {
            Content = fileEntiy.Content
        };
    }
}
