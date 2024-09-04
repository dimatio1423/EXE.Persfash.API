using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models.TokenModels
{
    public class TokenModel
    {
        public string userid { get; set; }

        public string roleName { get; set; }

        public string email { get; set; }

        public TokenModel(string userid, string roleName, string email)
        {
            this.userid = userid;
            this.roleName = roleName;
            this.email = email;
        }
    }
}
