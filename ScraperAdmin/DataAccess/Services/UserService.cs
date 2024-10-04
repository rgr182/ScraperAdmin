using BCrypt.Net;
using ScraperAdmin.DataAccess.Models;
using System;
using System.Collections.Generic;

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
            // Generar un token aleatorio para el usuario
            user.AccessToken = GenerateHashedPassword();

            // Guardar el usuario con la contraseña hasheada y el token generado
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

        // Método privado para generar una contraseña aleatoria y hashearla
        private string GenerateHashedPassword()
        {
            string randomPassword = Guid.NewGuid().ToString(); // Genera una contraseña aleatoria basada en GUID
            return BCrypt.Net.BCrypt.HashPassword(randomPassword); // Devuelve la contraseña hasheada
        }

        // Método privado para generar un token
        private string GenerateToken()
        {
            return Guid.NewGuid().ToString(); // Genera un token aleatorio basado en GUID
        }
    }
}
