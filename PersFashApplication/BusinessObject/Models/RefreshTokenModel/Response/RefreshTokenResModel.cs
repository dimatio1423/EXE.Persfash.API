using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models.RefreshTokenModel.Response
{
    public class RefreshTokenResModel
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
