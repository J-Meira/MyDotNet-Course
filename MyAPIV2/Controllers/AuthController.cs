using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MyAPIV2.Data;
using MyAPIV2.Dtos;
using MyAPIV2.Helpers;
using MyAPIV2.Models;
using System.Data;
using System.Security.Cryptography;

namespace MyAPIV2.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{

  private string _tableName = "TutorialAppSchema.Auth";
  private string _tableUsersName = "TutorialAppSchema.Users";

  private readonly AuthHelper _authHelper;
  private readonly DataContextDapper _connectionDapper;

  public AuthController(IConfiguration config)
  {
    _authHelper = new AuthHelper(config);
    _connectionDapper = new DataContextDapper(config);
  }

  [AllowAnonymous]
  [HttpPost("SignUp")]
  public IActionResult SignUp(UserSignUpDto user)
  {
    if (user.Email == "")
    {
      throw new Exception("Email is required");
    }
    if (user.Password == "")
    {
      throw new Exception("Password is required");
    }
    if (user.PasswordConfirm == "")
    {
      throw new Exception("PasswordConfirm is required");
    }
    if (user.PasswordConfirm != user.Password)
    {
      throw new Exception("Password do not match");
    }

    string sqlEmailCheck = "SELECT Email FROM "+
      $"{_tableName} WHERE Email = '{user.Email}'";

    IEnumerable<string> dbUsers = _connectionDapper.LoadData<string>(sqlEmailCheck);
    
    if(dbUsers.Count() > 0)
    {
      throw new Exception("A User with this email is already registred");
    }

    if (UpsertAuth(user.Email, user.Password))
    {
      string sqlInsert = "EXEC TutorialAppSchema.spUser_Upsert "+
        $"@FirstName = '{user.FirstName}', "+
        $"@LastName = '{user.LastName}', "+
        $"@Email = '{user.Email}', "+
        $"@Gender = '{user.Gender}', "+
        "@Active = '1', "+
        $"@JobTitle = '{user.JobTitle}', "+
        $"@Department = '{user.Department}', "+
        $"@Salary = '{user.Salary}'";
      Console.WriteLine(sqlInsert);
      if (_connectionDapper.ExecuteSql(sqlInsert))
      {
        return Ok();
      }
      throw new Exception("Fail to add user");
    }
    throw new Exception("Failed to register user.");
  }

  [AllowAnonymous]
  [HttpPost("SignIn")]
  public IActionResult SignIn(UserSignInDto user)
  {
    string sqlEmailCheck = "TutorialAppSchema.spLoginConfirmation_Get " +
      $"@Email = '{user.Email}'";

    UserSignInConfirmationDto confirmationUser = _connectionDapper
       .LoadSingleData<UserSignInConfirmationDto>(sqlEmailCheck);
    if (confirmationUser == null) {
      return BadRequest("Email or password invalid");
    }

    byte[] passwordHash = _authHelper.GetPasswordHash(user.Password, confirmationUser.PasswordSalt);

    // if (passwordHash == userForConfirmation.PasswordHash) // Won't work

    if(_authHelper.CompareTwoHashs(passwordHash,
      confirmationUser.PasswordHash) == false)
    {    
      return BadRequest("Email or password invalid");      
    }

    string userSql = $"SELECT * FROM {_tableUsersName} WHERE Email = '" +
               user.Email + "'";

    User userDb = _connectionDapper.LoadSingleData<User>(userSql);

    DateTime expires = DateTime.Now.AddDays(1);

    SignInRdto valueReturn = new SignInRdto(
      userDb,
      _authHelper.CreateToken(userDb.UserId, expires),
      expires
    );

    return Ok(valueReturn);
  }

  [HttpGet("RefreshToken")]
  public IActionResult RefreshToken()
  {
    string userIdSql = $"SELECT UserId FROM {_tableUsersName} WHERE UserId = '" +
        User.FindFirst("userId")?.Value + "'";

    int userId = _connectionDapper.LoadSingleData<int>(userIdSql);

    DateTime expires = DateTime.Now.AddDays(1);

    RefreshRdto valueReturn = new RefreshRdto(
      _authHelper.CreateToken(userId, expires),
      expires
    );

    return Ok(valueReturn);
  }

  [HttpPut("UpdatePassword")]
  public IActionResult UpdatePassword(UserUpdatePasswordDto user)
  {
    if (user.Email == "")
    {
      throw new Exception("Email is required");
    }
    if (user.Password == "")
    {
      throw new Exception("Password is required");
    }
    if (user.NewPassword == "")
    {
      throw new Exception("New Password is required");
    }
    if (user.NewPasswordConfirm == "")
    {
      throw new Exception("PasswordConfirm is required");
    }

    if (user.Password == user.NewPassword)
    {
      throw new Exception("Password and NewPassword are equals");
    }

    string sqlEmailCheck = "TutorialAppSchema.spLoginConfirmation_Get " +
      $"@Email = '{user.Email}'";

    UserSignInConfirmationDto confirmationUser = _connectionDapper
       .LoadSingleData<UserSignInConfirmationDto>(sqlEmailCheck);
    if (confirmationUser == null)
    {
      return BadRequest("Email invalid");
    }

    byte[] oldPasswordHash = _authHelper.GetPasswordHash(user.Password, confirmationUser.PasswordSalt);

    if (_authHelper.CompareTwoHashs(oldPasswordHash,
      confirmationUser.PasswordHash) == false)
    {
      return BadRequest("Incorrect password");
    }

    if (user.NewPasswordConfirm != user.NewPassword)
    {
      throw new Exception("NewPassword and NewPasswordConfirm  do not match");
    }

    if(UpsertAuth(user.Email, user.NewPassword))
    {
      return Ok();
    }    
    throw new Exception("Fail to update Password");
  }

  private bool UpsertAuth(string email, string password)
  {
    byte[] passwordSalt = new byte[128 / 8];
    using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
    {
      rng.GetNonZeroBytes(passwordSalt);
    }

    byte[] passwordHash = _authHelper.GetPasswordHash(password, passwordSalt);

    string sqlAddAuth = "EXEC TutorialAppSchema.spRegistration_Upsert " +
      "@Email = @EmailParam, " +
      "@PasswordHash = @PasswordHashParam, " +
      "@PasswordSalt = @PasswordSaltParam ";

    List<SqlParameter> sqlParameters = new List<SqlParameter>();

    SqlParameter emailParameter = new SqlParameter("@EmailParam", SqlDbType.VarChar);
    emailParameter.Value = email;

    SqlParameter passwordHashParameter = new SqlParameter("@PasswordHashParam", SqlDbType.VarBinary);
    passwordHashParameter.Value = passwordHash;

    SqlParameter passwordSaltParameter = new SqlParameter("@PasswordSaltParam", SqlDbType.VarBinary);
    passwordSaltParameter.Value = passwordSalt;    

    sqlParameters.Add(emailParameter);
    sqlParameters.Add(passwordHashParameter);
    sqlParameters.Add(passwordSaltParameter);

    return _connectionDapper.ExecuteSqlWithParameters(sqlAddAuth, sqlParameters);
  }
}
