using BusinessObject.Models.UserModels.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.AuthenticationServices;

namespace PersFashApplicationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginReqModel userLoginReqModel)
        {
            var result = await _authenticationService.Login(userLoginReqModel);
            return Ok(result);
        }
    }
}
