using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models.UserModels.Request
{
    public class UserLoginReqModel
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }

    public class UserLoginGoogleReqModel
    {
        public required string Token { get; set; }
    }
}
