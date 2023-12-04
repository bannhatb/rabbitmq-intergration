using ProducerService.API.Models.Entities;

namespace ProducerService.API.Repositories
{
    public interface IUserRepository
    {
        void Add(User user);
        void Update(User user);
        void Delete(User user);
        bool IsSaveChanges();
        User GetUserByUsername(string username);
        List<User> GetUsers();
        User GetUserById(int id);

    }
}