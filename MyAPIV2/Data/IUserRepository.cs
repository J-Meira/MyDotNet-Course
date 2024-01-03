using MyAPIV2.Models;

namespace MyAPIV2.Data
{
  public interface IUserRepository
  {
    public bool SaveChanges();
    public void AddEntity<T>(T entity);
    public void RemoveEntity<T>(T entity);
    public IEnumerable<User> GetAllUsers();
    public User GetUserById(int userId);
    public UserSalary GetUserSalaryById(int userId);
    public UserJobInfo GetUserJobInfoById(int userId);
  }
}
