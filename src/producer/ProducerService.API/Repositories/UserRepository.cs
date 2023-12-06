
using ProducerService.API.Models.Entities;

namespace ProducerService.API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _dataContext;

        public UserRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void Add(User user)
        {
            _dataContext.Users.Add(user);
        }

        public void Delete(User user)
        {
            _dataContext.Users.Remove(user);
        }

        public User GetUserById(int id)
        {
            return _dataContext.Users.FirstOrDefault(x => x.Id == id);
        }

        public User GetUserByUsername(string username)
        {
            return _dataContext.Users.FirstOrDefault(x => x.Username == username);
        }

        public List<User> GetUsers()
        {
            return _dataContext.Users.ToList();
        }

        public bool IsSaveChanges()
        {
            return _dataContext.SaveChanges() > 0;
        }

        public void Update(User user)
        {
            _dataContext.Users.Update(user);
        }
    }
}