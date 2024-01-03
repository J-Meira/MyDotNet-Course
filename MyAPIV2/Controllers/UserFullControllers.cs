using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using MyAPIV2.Data;
using MyAPIV2.Dtos;
using MyAPIV2.Models;

namespace MyAPIV2.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class UserFullController : ControllerBase
{
  private DataContextDapper _connectionDapper;
  public UserFullController(IConfiguration config)
  {
    _connectionDapper = new DataContextDapper(config);
  }

  [HttpPost("Add")]
  public IActionResult Add(UserFullDto user)
  {
    string sql = $@"EXEC TutorialAppSchema.spUser_Upsert
        @FirstName = '{user.FirstName}',
        @LastName = '{user.LastName}',
        @Email = '{user.Email}',
        @Gender = '{user.Gender}',
        @Active = 1,
        @JobTitle = '{user.JobTitle}',
        @Department = '{user.Department}',
        @Salary = '{user.Salary}'";

    if (_connectionDapper.ExecuteSql(sql))
    {
      return Ok();
    }
    throw new Exception("Fail to add user");
  }

  [HttpGet("GetAll")]
  public IEnumerable<UserFull> GetAll(bool? isActive)
  {
    string isActiveSql = "";
    if (isActive != null)
    {
      isActiveSql = " @Active= " + isActive.ToString();
    }
    string sql = "EXEC TutorialAppSchema.spUsers_Get" + isActiveSql;
    return _connectionDapper.LoadData<UserFull>(sql);
  }

  [HttpGet("GetById/{userId}")]
  public UserFull GetById(int userId)
  {
    string sql = $"EXEC TutorialAppSchema.spUsers_Get @UserId = {userId}";
    return _connectionDapper.LoadSingleData<UserFull>(sql);
  }

  [HttpPut("UpdateById/{userId}")]
  public IActionResult UpdateById(UserFullDto user, int userId)
  {
    UserFull userDb = _connectionDapper.LoadSingleData<UserFull>("EXEC TutorialAppSchema.spUsers_Get @UserId = "+
      userId.ToString());

    if (userDb == null)
    {
      throw new Exception("Fail to update user");
    }
    string sql = "EXEC TutorialAppSchema.spUser_Upsert " +
     $@"@FirstName = '{user.FirstName}',
        @LastName = '{user.LastName}',
        @Email = '{user.Email}',
        @Gender = '{user.Gender}',
        @Active = '{userDb.Active}',
        @JobTitle = '{user.JobTitle}',
        @Department = '{user.Department}',
        @Salary = '{user.Salary}',
        @UserId = {userId}";

    if (_connectionDapper.ExecuteSql(sql))
    {
      return Ok();
    }
    throw new Exception("Fail to update user");
  }

  [HttpDelete("DeleteById/{userId}")]
  public IActionResult DeleteById(int userId)
  {
    string sql = "EXEC TutorialAppSchema.spUser_Delete " +
      $"@UserId = {userId}";

    if (_connectionDapper.ExecuteSql(sql))
    {
      return Ok();
    }
    throw new Exception("Fail to delete user");
  }

  [HttpPut("ActiveInactiveById/{userId}")]
  public IActionResult ActiveInactiveById(int userId, bool status)
  {
    UserFull userDb = _connectionDapper.LoadSingleData<UserFull>("EXEC TutorialAppSchema.spUsers_Get @UserId = " +
      userId.ToString());
    Console.WriteLine(userDb.UserId);
    if (userDb == null)
    {
      throw new Exception("Fail to update user");
    }
    string sql = "EXEC TutorialAppSchema.spUser_Upsert " +
     $@"@FirstName = '{userDb.FirstName}',
        @LastName = '{userDb.LastName}',
        @Email = '{userDb.Email}',
        @Gender = '{userDb.Gender}',
        @Active = '{status}',
        @JobTitle = '{userDb.JobTitle}',
        @Department = '{userDb.Department}',
        @Salary = '{userDb.Salary}',
        @UserId = {userId}";

    if (_connectionDapper.ExecuteSql(sql))
    {
      return Ok();
    }
    throw new Exception("Fail to update user");
  }
}
