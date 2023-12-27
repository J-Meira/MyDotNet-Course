using Microsoft.AspNetCore.Mvc;
using AutoMapper;

using MyAPIV2.Data;
using MyAPIV2.Dtos;
using MyAPIV2.Models;

namespace MyAPIV2.Controllers;

[ApiController]
[Route("[controller]")]
public class UserEFController : ControllerBase
{
  private DataContextEF _connectionEF;
  private IMapper _mapper;
  public UserEFController(IConfiguration config)
  {
    _connectionEF = new DataContextEF(config);
    _mapper = new Mapper(new MapperConfiguration(cfg =>
    {
      cfg.CreateMap<UserDto, UserEFController>();
    }));

  }

  [HttpPost("Add")]
  public IActionResult Add(UserDto user)
  {
    User userDb = _mapper.Map<User>(user);

    _connectionEF.Add(userDb);
    if (_connectionEF.SaveChanges() > 0)
    {
      return Ok();
    }
    throw new Exception("Fail to add user");
  }

  [HttpGet("GetAll")]
  public IEnumerable<User> GetAll()
  {
    IEnumerable<User> users = _connectionEF.Users.ToList<User>();
    return users;
  }

  [HttpGet("GetById/{userId}")]
  public User GetById(int userId)
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


  [HttpPut("UpdateById/{userId}")]
  public IActionResult UpdateById(UserDto user, int userId)
  {
    User? userDb = _connectionEF.Users
      .Where(u => u.UserId == userId)
      .FirstOrDefault<User>();

    if (userDb != null)
    {
      userDb.FirstName = user.FirstName;
      userDb.LastName = user.LastName;
      userDb.Email = user.Email;
      userDb.Gender = user.Gender;
      userDb.Active = user.Active;
      if (_connectionEF.SaveChanges() > 0)
      {
        return Ok();
      }
      throw new Exception("Fail to update user");
    }
    throw new Exception("Fail to update user");
  }

  [HttpDelete("DeleteById/{userId}")]
  public IActionResult DeleteById(int userId)
  {
    User? userDb = _connectionEF.Users
      .Where(u => u.UserId == userId)
      .FirstOrDefault<User>();

    if (userDb != null)
    {
      _connectionEF.Users.Remove(userDb);
      if (_connectionEF.SaveChanges() > 0)
      {
        return Ok();
      }
      throw new Exception("Fail to delete user");
    }
    throw new Exception("Fail to delete user");
  }

  [HttpPost("AddSalary")]
  public IActionResult AddSalary(UserSalary user)
  {
    _connectionEF.UserSalary.Add(user);
    if (_connectionEF.SaveChanges() > 0)
    {
      return Ok();
    }
    throw new Exception("Fail to add user SALARY");
  }

  [HttpGet("GetSalaryById/{userId}")]
  public UserSalary GetSalaryById(int userId)
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
  
[HttpPut("UpdateSalaryById/{userId}")]
  public IActionResult UpdateSalaryById(UserSalary user, int userId)
  {
    UserSalary? userDb = _connectionEF.UserSalary
      .Where(u => u.UserId == userId)
      .FirstOrDefault<UserSalary>();

    if (userDb != null)
    {
      userDb.Salary = user.Salary;
      if (_connectionEF.SaveChanges() > 0)
      {
        return Ok();
      }
      throw new Exception("Fail to update user salary");
    }
    throw new Exception("Fail to update user salary");
  }

  [HttpDelete("DeleteSalaryById/{userId}")]
  public IActionResult DeleteSalaryById(int userId)
  {
    UserSalary? userDb = _connectionEF.UserSalary
      .Where(u => u.UserId == userId)
      .FirstOrDefault<UserSalary>();

    if (userDb != null)
    {
      _connectionEF.UserSalary.Remove(userDb);
      if (_connectionEF.SaveChanges() > 0)
      {
        return Ok();
      }
      throw new Exception("Fail to delete user Salary");
    }
    throw new Exception("Fail to delete user Salary");
  }

  [HttpPost("AddJobInfo")]
  public IActionResult AddJobInfo(UserJobInfo user)
  {
    _connectionEF.UserJobInfo.Add(user);
    if (_connectionEF.SaveChanges() > 0)
    {
      return Ok();
    }
    throw new Exception("Fail to add user JobInfo");
  }

  [HttpGet("GetJobInfoById/{userId}")]
  public UserJobInfo GetJobInfoById(int userId)
  {
    UserJobInfo? userDb = _connectionEF.UserJobInfo
      .Where(u => u.UserId == userId)
      .FirstOrDefault<UserJobInfo>();

    if (userDb != null)
    {
      return userDb;
    }
    throw new Exception("Fail to get user JobInfo");
  }
  
[HttpPut("UpdateJobInfoById/{userId}")]
  public IActionResult UpdateJobInfoById(UserJobInfo user, int userId)
  {
    UserJobInfo? userDb = _connectionEF.UserJobInfo
      .Where(u => u.UserId == userId)
      .FirstOrDefault<UserJobInfo>();

    if (userDb != null)
    {
      userDb.JobTitle = user.JobTitle;
      userDb.Department = user.Department;
      if (_connectionEF.SaveChanges() > 0)
      {
        return Ok();
      }
      throw new Exception("Fail to update user JobInfo");
    }
    throw new Exception("Fail to update user JobInfo");
  }

  [HttpDelete("DeleteJobInfoById/{userId}")]
  public IActionResult DeleteJobInfoById(int userId)
  {
    UserJobInfo? userDb = _connectionEF.UserJobInfo
      .Where(u => u.UserId == userId)
      .FirstOrDefault<UserJobInfo>();

    if (userDb != null)
    {
      _connectionEF.UserJobInfo.Remove(userDb);
      if (_connectionEF.SaveChanges() > 0)
      {
        return Ok();
      }
      throw new Exception("Fail to delete user JobInfo");
    }
    throw new Exception("Fail to delete user JobInfo");
  }

}