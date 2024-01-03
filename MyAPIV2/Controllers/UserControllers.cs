using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using MyAPIV2.Data;
using MyAPIV2.Dtos;
using MyAPIV2.Models;

namespace MyAPIV2.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
  private string _tableName = "TutorialAppSchema.Users";
  private string _salaryTableName = "TutorialAppSchema.UserSalary";
  private string _jobInfoTableJobName = "TutorialAppSchema.UserJobInfo";

  private DataContextDapper _connectionDapper;
  public UserController(IConfiguration config)
  {
    _connectionDapper = new DataContextDapper(config);
  }  

  [HttpPost("Add")]
  public IActionResult Add(UserDto user)
  {
    string sql = $@"INSERT INTO {_tableName}
      (FirstName, LastName, Email, Gender, Active)
    VALUES(
        '{user.FirstName}',
        '{user.LastName}',
        '{user.Email}',
        '{user.Gender}',
        1)";

    if(_connectionDapper.ExecuteSql(sql)){
      return Ok();
    }
    throw new Exception("Fail to add user");
  }

  [HttpGet("GetAll")]
  public IEnumerable<User> GetAll(string? orderBy, string? desc)
  {
    string sqlOrderBy = " ORDER BY UserId";
    string sqlDesc = "";
    if(orderBy != null)
    {
      sqlOrderBy = $" ORDER BY {orderBy}";
    }
    if(desc != null && desc.ToUpper() == "TRUE")
    {
      sqlDesc = " DESC";
    }
    return _connectionDapper.LoadData<User>($"SELECT * FROM {_tableName}"+sqlOrderBy+sqlDesc);
  }

  [HttpGet("GetById/{userId}")]
  public User GetById(int userId)
  {
    string sql = $"SELECT * FROM {_tableName} WHERE [UserId] = {userId}";
    return _connectionDapper.LoadSingleData<User>(sql);
  }

  [HttpPut("UpdateById/{userId}")]
  public IActionResult UpdateById(UserDto user, int userId)
  {
    User userDb = _connectionDapper.LoadSingleData<User>($"SELECT * FROM {_tableName} WHERE [UserId] = {userId}");
    string sql = $@"UPDATE {_tableName}
    SET
        [FirstName] = '{user.FirstName}',
        [LastName] = '{user.LastName}',
        [Email] = '{user.Email}',
        [Gender] = '{user.Gender}',
        [Active] = '{userDb.Active}'
    WHERE [UserId] = {userId}";

    if(_connectionDapper.ExecuteSql(sql)){
      return Ok();
    }
    throw new Exception("Fail to update user");
  }

  [HttpDelete("DeleteById/{userId}")]
  public IActionResult DeleteById(int userId)
  {
    string sql = $@"DELETE {_tableName}
    WHERE [UserId] = {userId}";

    if(_connectionDapper.ExecuteSql(sql)){
      return Ok();
    }
    throw new Exception("Fail to delete user");
  }

    [HttpPost("AddSalary")]
  public IActionResult AddSalary(UserSalary user)
  {
    string sql = $@"INSERT INTO {_salaryTableName}
      (UserId, Salary)
    VALUES(
        '{user.UserId}',
        '{user.Salary}'
    )";

    if(_connectionDapper.ExecuteSql(sql)){
      return Ok();
    }
    throw new Exception("Fail to add user salary");
  }

    [HttpGet("GetSalaryById/{userId}")]
  public UserSalary GetSalaryById(int userId)
  {
    string sql = $"SELECT * FROM {_salaryTableName} WHERE [UserId] = {userId}";
    return _connectionDapper.LoadSingleData<UserSalary>(sql);
  }

  [HttpPut("UpdateSalaryById/{userId}")]
  public IActionResult UpdateSalaryById(UserSalaryDto user, int userId)
  {
    string sql = $@"UPDATE {_salaryTableName}
    SET
        [Salary] = '{user.Salary}',
    WHERE [UserId] = {userId}";

    if(_connectionDapper.ExecuteSql(sql)){
      return Ok();
    }
    throw new Exception("Fail to update user salary");
  }

  [HttpDelete("DeleteSalaryById/{userId}")]
  public IActionResult DeleteSalaryById(int userId)
  {
    string sql = $@"DELETE {_salaryTableName}
    WHERE [UserId] = {userId}";

    if(_connectionDapper.ExecuteSql(sql)){
      return Ok();
    }
    throw new Exception("Fail to delete user salary");
  }

    [HttpPost("AddJobInfo")]
  public IActionResult AddJobInfo(UserJobInfo user)
  {
    string sql = $@"INSERT INTO {_jobInfoTableJobName}
      (UserId, JobTitle, Department)
    VALUES(
        {user.UserId},
        '{user.JobTitle}',
        '{user.Department}'
    )";

    if(_connectionDapper.ExecuteSql(sql)){
      return Ok();
    }
    throw new Exception("Fail to add user salary");
  }

    [HttpGet("GetJobInfoById/{userId}")]
  public UserJobInfo GetJobInfoById(int userId)
  {
    string sql = $"SELECT * FROM {_jobInfoTableJobName} WHERE [UserId] = {userId}";
    return _connectionDapper.LoadSingleData<UserJobInfo>(sql);
  }

  [HttpPut("UpdateJobInfoById/{userId}")]
  public IActionResult UpdateJobInfoById(UserJobInfoDto user, int userId)
  {
    string sql = $@"UPDATE {_jobInfoTableJobName}
    SET
        [JobTitle] = '{user.JobTitle}',
        [Department] = '{user.Department}'
    WHERE [UserId] = {userId}";

    if(_connectionDapper.ExecuteSql(sql)){
      return Ok();
    }
    throw new Exception("Fail to update user salary");
  }

  [HttpDelete("DeleteJobInfoById/{userId}")]
  public IActionResult DeleteJobInfoById(int userId)
  {
    string sql = $@"DELETE {_jobInfoTableJobName}
    WHERE [UserId] = {userId}";

    if(_connectionDapper.ExecuteSql(sql)){
      return Ok();
    }
    throw new Exception("Fail to delete user salary");
  }

}
