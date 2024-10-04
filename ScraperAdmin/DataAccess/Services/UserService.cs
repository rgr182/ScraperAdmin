using ScraperAdmin.DataAccess.Models;

namespace ScraperAdmin.DataAccess.Services
{
    public interface IUserService
    {
        IEnumerable<Users> GetAllUsers();
        Users GetUserById(int id);
        void AddUser(Users user);
        void UpdateUser(Users user);
        void DeleteUser(int id);
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IEnumerable<Users> GetAllUsers()
        {
            return _userRepository.GetAllUsers();
        }

        public Users GetUserById(int id)
        {
            return _userRepository.GetUserById(id);
        }

        public void AddUser(Users user)
        {
            // Generate a random token for the user (you can replace this with any token logic)
            string token = Guid.NewGuid().ToString();

            // Hash the token using BCrypt
            user.AccessToken = BCrypt.Net.BCrypt.HashPassword(token);

            // Save the user with the hashed token
            _userRepository.AddUser(user);
        }

        public void UpdateUser(Users user)
        {
            _userRepository.UpdateUser(user);
        }

        public void DeleteUser(int id)
        {
            _userRepository.DeleteUser(id);
        }
    }
}
