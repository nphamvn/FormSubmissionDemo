using FormSubmissionDemo.Services;
using Microsoft.AspNetCore.Mvc;

namespace FormSubmissionDemo.Controllers;

public class FileController(IHttpContextAccessor httpContextAccessor) : BaseController(httpContextAccessor)
{
    [HttpGet("File/{FileId:int}")]
    public async Task<IActionResult> File(int FileId)
    {
        var file = await AppFileService.Get(FileId);
        return new FileStreamResult(new MemoryStream(file.Content), "file");
    }

    [HttpGet("TempFile/{FileId:int}")]
    public async Task<IActionResult> TempFile(int FileId)
    {
        var file = await TempFileService.Get(FileId);
        return new FileStreamResult(new MemoryStream(file.Content), "file");
    }
}