using MyAPIV2.Models;
using System.Linq;

namespace MyAPIV2.Data
{
  public class UserRepository : IUserRepository
  {

    private DataContextEF _connectionEF;

    public UserRepository(IConfiguration config)
    {
      _connectionEF = new DataContextEF(config);
    }

    public bool SaveChanges()
    {
      return _connectionEF.SaveChanges() >0;
    }

    public void AddEntity<T>(T entity)
    {
      if(entity != null)
      {
        _connectionEF.Add(entity);
      }
    }
    
    public void RemoveEntity<T>(T entity)
    {
      if(entity != null)
      {
        _connectionEF.Remove(entity);
      }
    }

    public IEnumerable<User> GetAllUsers()
    {
      return _connectionEF.Users.ToList<User>();
    }

    public User GetUserById(int userId)
    {
      User? userDb = _connectionEF.Users
        .Where(u => u.UserId == userId)
        .FirstOrDefault<User>();

      if (userDb != null)
      {
        return userDb;
      }
      throw new Exception("Fail to get user");
    }

    public UserSalary GetUserSalaryById(int userId)
    {
      UserSalary? userDb = _connectionEF.UserSalary
        .Where(u => u.UserId == userId)
        .FirstOrDefault<UserSalary>();

      if (userDb != null)
      {
        return userDb;
      }
      throw new Exception("Fail to get user salary");
    }

    public UserJobInfo GetUserJobInfoById(int userId)
    {
      UserJobInfo? userDb = _connectionEF.UserJobInfo
        .Where(u => u.UserId == userId)
        .FirstOrDefault<UserJobInfo>();

      if (userDb != null)
      {
        return userDb;
      }
      throw new Exception("Fail to get user job info");
    }


  }
}