using BusinessObject.Models.UserModels.Request;
using BusinessObjects.Models.RefreshTokenModel.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.RefreshTokenServices;

namespace PersFashApplicationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RefreshTokenController : ControllerBase
    {
        private readonly IRefreshTokenService _refreshTokenService;

        public RefreshTokenController(IRefreshTokenService refreshTokenService)
        {
            _refreshTokenService = refreshTokenService;
        }
        [HttpPost]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenReqModel refreshTokenReqModel)
        {
            var result = await _refreshTokenService.RefreshToken(refreshTokenReqModel);
            return Ok(result);
        }
    }
}
