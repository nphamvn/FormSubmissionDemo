using FormSubmissionDemo.Entities;

namespace FormSubmissionDemo.Data.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserEntity>> GetAll();
        Task<UserEntity> Get(int userId);
        Task<int> Add(UserEntity userEntity);
        Task Update(int userId, UserEntity userEntity);
    }
    public class UserRepository : IUserRepository
    {
        public Task<IEnumerable<UserEntity>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<UserEntity> Get(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<int> Add(UserEntity userEntity)
        {
            throw new NotImplementedException();
        }

        public Task Update(int userId, UserEntity userEntity)
        {
            throw new NotImplementedException();
        }
    }
}