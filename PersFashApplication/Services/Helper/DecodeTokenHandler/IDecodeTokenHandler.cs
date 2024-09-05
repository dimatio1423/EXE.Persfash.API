using BusinessObject.Models.TokenModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Helpers.Handler.DecodeTokenHandler
{
    public interface IDecodeTokenHandler
    {
        TokenModel decode(string token);

    }
}
