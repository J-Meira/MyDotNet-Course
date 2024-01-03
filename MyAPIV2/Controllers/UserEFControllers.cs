using Microsoft.AspNetCore.Mvc;
using AutoMapper;

using MyAPIV2.Data;
using MyAPIV2.Dtos;
using MyAPIV2.Models;
using Microsoft.AspNetCore.Authorization;

namespace MyAPIV2.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class UserEFController : ControllerBase
{
  private IMapper _mapper;
  private IUserRepository _userRepository;
  public UserEFController(IConfiguration config, IUserRepository userRepository)
  {
    _mapper = new Mapper(new MapperConfiguration(cfg =>
    {
      cfg.CreateMap<UserDto, UserEFController>();
    }));
    _userRepository = userRepository;
  }

  [HttpPost("Add")]
  public IActionResult Add(UserDto user)
  {
    User userDb = _mapper.Map<User>(user);

    _userRepository.AddEntity<User>(userDb);
    if (_userRepository.SaveChanges())
    {
      return Ok();
    }
    throw new Exception("Fail to add user");
  }

  [HttpGet("GetAll")]
  public IEnumerable<User> GetAll()
  {
    return _userRepository.GetAllUsers();
  }

  [HttpGet("GetById/{userId}")]
  public User GetById(int userId)
  {
    return _userRepository.GetUserById(userId);
  }

  [HttpPut("UpdateById/{userId}")]
  public IActionResult UpdateById(UserDto user, int userId)
  {
    User? userDb = _userRepository.GetUserById(userId);

    if (userDb != null)
    {
      userDb.FirstName = user.FirstName;
      userDb.LastName = user.LastName;
      userDb.Email = user.Email;
      userDb.Gender = user.Gender;
      userDb.Active = userDb.Active;
      if (_userRepository.SaveChanges())
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
    User? userDb = _userRepository.GetUserById(userId);

    if (userDb != null)
    {
      _userRepository.RemoveEntity<User>(userDb);
      if (_userRepository.SaveChanges())
      {
        return Ok();
      }
      throw new Exception("Fail to delete user");
    }
    throw new Exception("Fail to delete user");
  }

  [HttpPost("AddUserSalary")]
  public IActionResult AddUserSalary(UserSalary user)
  {
    _userRepository.AddEntity<UserSalary>(user);
    if (_userRepository.SaveChanges())
    {
      return Ok();
    }
    throw new Exception("Fail to add user SALARY");
  }

  [HttpGet("GetUserSalaryById/{userId}")]
  public UserSalary GetUserSalaryById(int userId)
  {
    UserSalary? userDb = _userRepository.GetUserSalaryById(userId);

    if (userDb != null)
    {
      return userDb;
    }
    throw new Exception("Fail to get user salary");
  }
  
[HttpPut("UpdateUserSalaryById/{userId}")]
  public IActionResult UpdateUserSalaryById(UserSalaryDto user, int userId)
  {
    UserSalary? userDb = _userRepository.GetUserSalaryById(userId);

    if (userDb != null)
    {
      userDb.Salary = user.Salary;
      if (_userRepository.SaveChanges())
      {
        return Ok();
      }
      throw new Exception("Fail to update user salary");
    }
    throw new Exception("Fail to update user salary");
  }

  [HttpDelete("DeleteUserSalaryById/{userId}")]
  public IActionResult DeleteUserSalaryById(int userId)
  {
    UserSalary? userDb = _userRepository.GetUserSalaryById(userId);

    if (userDb != null)
    {
      _userRepository.RemoveEntity<UserSalary>(userDb);
      if (_userRepository.SaveChanges())
      {
        return Ok();
      }
      throw new Exception("Fail to delete user Salary");
    }
    throw new Exception("Fail to delete user Salary");
  }

  [HttpPost("AddUserJobInfo")]
  public IActionResult AddUserJobInfo(UserJobInfo user)
  {
    _userRepository.AddEntity<UserJobInfo>(user);
    if (_userRepository.SaveChanges())
    {
      return Ok();
    }
    throw new Exception("Fail to add user JobInfo");
  }

  [HttpGet("GetUserJobInfoById/{userId}")]
  public UserJobInfo GetUserJobInfoById(int userId)
  {
    UserJobInfo? userDb = _userRepository.GetUserJobInfoById(userId);
    if (userDb != null)
    {
      return userDb;
    }
    throw new Exception("Fail to get user JobInfo");
  }
  
[HttpPut("UpdateUserJobInfoById/{userId}")]
  public IActionResult UpdateUserJobInfoById(UserJobInfoDto user, int userId)
  {
    UserJobInfo? userDb = _userRepository.GetUserJobInfoById(userId);

    if (userDb != null)
    {
      userDb.JobTitle = user.JobTitle;
      userDb.Department = user.Department;
      if (_userRepository.SaveChanges())
      {
        return Ok();
      }
      throw new Exception("Fail to update user JobInfo");
    }
    throw new Exception("Fail to update user JobInfo");
  }

  [HttpDelete("DeleteUserJobInfoById/{userId}")]
  public IActionResult DeleteUserJobInfoById(int userId)
  {
    UserJobInfo? userDb = _userRepository.GetUserJobInfoById(userId);

    if (userDb != null)
    {
      _userRepository.RemoveEntity<UserJobInfo>(userDb);
      if (_userRepository.SaveChanges())
      {
        return Ok();
      }
      throw new Exception("Fail to delete user JobInfo");
    }
    throw new Exception("Fail to delete user JobInfo");
  }

}