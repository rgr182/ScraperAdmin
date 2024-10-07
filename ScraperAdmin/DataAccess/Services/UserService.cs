using BCrypt.Net;
using ScraperAdmin.DataAccess.Models;
using ScraperAdmin.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ScraperAdmin.DataAccess.Services
{
    public interface IUserService
    {
        Task<IEnumerable<Users>> GetAllUsersAsync();
        Task<Users> GetUserByIdAsync(int id);
        Task AddUserAsync(Users user);
        Task UpdateUserAsync(Users user);
        Task DeleteUserAsync(int id);
        Task<bool> ValidateTokenAsync(string token);
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<Users>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }

        public async Task<Users> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetUserByIdAsync(id);
        }

        public async Task AddUserAsync(Users user)
        {
            // Generar un token aleatorio para el usuario
            user.AccessToken = GenerateToken();

            // Guardar el usuario con la contraseña hasheada y el token generado
            await _userRepository.AddUserAsync(user);
        }

        public async Task UpdateUserAsync(Users user)
        {
            await _userRepository.UpdateUserAsync(user);
        }

        public async Task DeleteUserAsync(int id)
        {
            await _userRepository.DeleteUserAsync(id);
        }

        public async Task<bool> ValidateTokenAsync(string token)
        {
            var user = await _userRepository.GetUserByTokenAsync(token);
            return user != null; // Si se encuentra el usuario con ese token, el token es válido
        }

        // Método privado para generar un token
        private string GenerateToken()
        {
            return Guid.NewGuid().ToString(); // Genera un token aleatorio basado en GUID
        }
    }
}
