using MyAPIV2.Models;

namespace MyAPIV2.Dtos
{
  public partial class SignInRdto
  {
    public User? User { get; set; }
    public string AccessToken { get; set; }
    public DateTime ExpiresIn { get; set; }

    public SignInRdto(User? user, string accessToken, DateTime expiresIn)
    {
      User = user;
      AccessToken = accessToken;
      ExpiresIn = expiresIn;
    }
  }
}
