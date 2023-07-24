namespace FormSubmissionDemo.Services;

public interface IUserService
{

}
public class UserService : BaseService, IUserService
{
    public UserService(IAppFileService appFileService
        , ITempFileService tempFileService) : base(appFileService, tempFileService)
    {
    }
}