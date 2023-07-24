namespace FormSubmissionDemo.Services;
public class BaseService
{
    private readonly IAppFileService _appFileService;
    private readonly ITempFileService _tempFileService;

    public BaseService(IAppFileService appFileService, ITempFileService tempFileService)
    {
        _appFileService = appFileService;
        _tempFileService = tempFileService;
    }
}
