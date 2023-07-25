using FormSubmissionDemo.Models.Shared;

namespace FormSubmissionDemo.Models.Users;
public class UserIndexViewModel
{
    public PaginatedList<UserModel> Users { get; set; }
}