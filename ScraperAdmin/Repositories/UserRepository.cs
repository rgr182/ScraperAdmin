using ScraperAdmin.DataAccess.Context;
using ScraperAdmin.DataAccess.Models;

public interface IUserRepository
{
    IEnumerable<Users> GetAllUsers();
    Users GetUserById(int id);
    Users GetUserByToken(string token);
    void AddUser(Users user);
    void UpdateUser(Users user);
    void DeleteUser(int id);
}

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Users> GetAllUsers()
    {
        return _context.Users;
    }

    public Users GetUserById(int id)
    {
        return _context.Users.Find(id);
    }

    public Users GetUserByToken(string token)
    {
        return _context.Users.FirstOrDefault(u => u.AccessToken == token);
    }

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

    public void UpdateUser(Users user)
    {
        _context.Users.Update(user);
        _context.SaveChanges();
    }

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
