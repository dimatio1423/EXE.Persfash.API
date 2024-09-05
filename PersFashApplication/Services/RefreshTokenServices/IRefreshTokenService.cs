using BusinessObjects.Models.RefreshTokenModel.Request;
using BusinessObjects.Models.RefreshTokenModel.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.RefreshTokenServices
{
    public interface IRefreshTokenService
    {
        Task<RefreshTokenResModel> RefreshToken(RefreshTokenReqModel refreshTokenReqModel);
    }
}
