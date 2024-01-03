using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MyAPIV2.Dtos
{
  public partial class UserSignUpDto : UserFullDto
  {
    public string Password { get; set; } = "";
    public string PasswordConfirm { get; set; } = "";
  }
}
