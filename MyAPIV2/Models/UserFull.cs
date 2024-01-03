namespace MyAPIV2.Models
{
  public partial class UserFull : User
  {
    public string JobTitle { get; set; } = "";
    public string Department { get; set; } = "";
    public decimal Salary { get; set; }
    public decimal AvgSalary { get; set; }
  }
}
