
using System.Collections.Generic;
using ScraperAdmin.DataAccess.Context;
using ScraperAdmin.DataAccess.Models;

namespace ScraperAdmin.DataAccess.Services
{
    /// <summary>
    /// Implementation of IUserRepository for managing user data.
    /// </summary>
    
    public interface IUserRepository
    {
        IEnumerable<Users> GetAllUsers();
        Users GetUserById(int id);
        void AddUser(Users user);
        void UpdateUser(Users user);
        void DeleteUser(int id);
    }

    public class UserRepository : IUserRepository
    
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Constructor to initialize the UserRepository with a database context.
        /// </summary>
        /// <param name="context">The database context to be used.</param>
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all users from the database.
        /// </summary>
        /// <returns>A collection of users.</returns>
        public IEnumerable<Users> GetAllUsers()
        {
            return _context.Users;
        }

        /// <summary>
        /// Retrieves a user by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the user.</param>
        /// <returns>A User object if found, otherwise null.</returns>
        public Users GetUserById(int id)
        {
            return _context.Users.Find(id);
        }

        /// <summary>
        /// Adds a new user to the database.
        /// </summary>
        /// <param name="user">The user entity to add.</param>
        public void AddUser(Users user)
        {
            try
            {
                _context.Users.Add(user);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        
        }

        /// <summary>
        /// Updates an existing user in the database.
        /// </summary>
        /// <param name="user">The user entity with updated information.</param>
        public void UpdateUser(Users user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        /// <summary>
        /// Deletes a user from the database by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the user to delete.</param>
        public void DeleteUser(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }
    }
}
