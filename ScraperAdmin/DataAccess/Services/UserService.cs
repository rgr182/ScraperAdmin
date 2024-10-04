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
        bool ValidateToken(string token);
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
            // Generar un token aleatorio para el usuario (puedes reemplazarlo por tu lógica de generación de token)
            string token = Guid.NewGuid().ToString();

            // Guardar el token directamente en la base de datos
            user.AccessToken = token;

            // Guardar el usuario con el token generado
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

        public bool ValidateToken(string token)
        {
            var user = _userRepository.GetUserByToken(token);
            return user != null; // Si se encuentra el usuario con ese token, el token es válido
        }
    }
}
