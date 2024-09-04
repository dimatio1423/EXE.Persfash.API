using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models.UserModels.Response
{
    public class UserLoginResModel
    {
        public int UserId { get; set; }

        public string Username { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string? Role { get; set; }

        public string Token { get; set; }

        public string RefreshToken { get; set; }
    }
}
