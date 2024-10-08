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
            // Generate a random token for the user
            user.AccessToken = GenerateToken();

            // Save the user with the hashed password and generated token
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
            return user != null; // If a user is found with that token, the token is valid
        }

        // Private method to generate a token
        private string GenerateToken()
        {
            return Guid.NewGuid().ToString(); // Generates a random token based on GUID
        }
    }
}
