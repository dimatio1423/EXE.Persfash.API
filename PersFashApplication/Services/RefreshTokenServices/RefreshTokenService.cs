using BusinessObjects.Models.RefreshTokenModel.Request;
using BusinessObjects.Models.RefreshTokenModel.Response;
using Repositories.RefreshTokenRepos;
using Services.Helper.CustomExceptions;
using Services.JWTService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Services.RefreshTokenServices
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IJWTService _jWTService;

        public RefreshTokenService(IRefreshTokenRepository refreshTokenRepository, IJWTService jWTService)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _jWTService = jWTService;
        }
        public async Task<RefreshTokenResModel> RefreshToken(RefreshTokenReqModel refreshTokenReqModel)
        {
            var currRefreshToken = await _refreshTokenRepository.GetByRefreshToken(refreshTokenReqModel.RefreshToken);

            if (currRefreshToken == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Refresh token not found");
            }

            if (currRefreshToken.ExpiredAt < DateTime.Now)
            {
                throw new ApiException(HttpStatusCode.BadRequest, "Refresh token is expired");
            }

            var currCustomer = currRefreshToken.Customer;
            var currPartner = currRefreshToken.Partner;
            var currInfluencer = currRefreshToken.Influencer;
            var currAdmin = currRefreshToken.Admin;

            if (currCustomer != null)
            {
                var token = _jWTService.GenerateJWT(currCustomer);

                var newRefreshToken = _jWTService.GenerateRefreshToken();

                currRefreshToken.Token = newRefreshToken;
                currRefreshToken.ExpiredAt = DateTime.Now.AddDays(1);
                await _refreshTokenRepository.Update(currRefreshToken);

                return new RefreshTokenResModel
                {
                    AccessToken = token,
                    RefreshToken = newRefreshToken
                };
            }else if (currPartner != null)
            {
                var token = _jWTService.GenerateJWT(currPartner);

                var newRefreshToken = _jWTService.GenerateRefreshToken();

                currRefreshToken.Token = newRefreshToken;
                currRefreshToken.ExpiredAt = DateTime.Now.AddDays(1);
                await _refreshTokenRepository.Update(currRefreshToken);

                return new RefreshTokenResModel
                {
                    AccessToken = token,
                    RefreshToken = newRefreshToken
                };
            }else if (currInfluencer != null)
            {
                var token = _jWTService.GenerateJWT(currInfluencer);

                var newRefreshToken = _jWTService.GenerateRefreshToken();

                currRefreshToken.Token = newRefreshToken;
                currRefreshToken.ExpiredAt = DateTime.Now.AddDays(1);
                await _refreshTokenRepository.Update(currRefreshToken);

                return new RefreshTokenResModel
                {
                    AccessToken = token,
                    RefreshToken = newRefreshToken
                };
            }else if (currAdmin != null)
            {
                var token = _jWTService.GenerateJWT(currAdmin);

                var newRefreshToken = _jWTService.GenerateRefreshToken();

                currRefreshToken.Token = newRefreshToken;
                currRefreshToken.ExpiredAt = DateTime.Now.AddDays(1);
                await _refreshTokenRepository.Update(currRefreshToken);

                return new RefreshTokenResModel
                {
                    AccessToken = token,
                    RefreshToken = newRefreshToken
                };
            }
            
            else
            {
                throw new ApiException(HttpStatusCode.NotFound, "User does not exist");
            }
        }
    }
}
