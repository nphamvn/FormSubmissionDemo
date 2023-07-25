using FormSubmissionDemo.Entities;

namespace FormSubmissionDemo.Data.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserEntity>> GetAll();
    }
    public class UserRepository : IUserRepository
    {
        public Task<IEnumerable<UserEntity>> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}