namespace MyAPIV2.Dtos
{
  public partial class RefreshRdto
  {
    public string AccessToken { get; set; }
    public DateTime ExpiresIn { get; set; }

    public RefreshRdto(string accessToken, DateTime expiresIn)
    {
      AccessToken = accessToken;
      ExpiresIn = expiresIn;
    }
  }
}
