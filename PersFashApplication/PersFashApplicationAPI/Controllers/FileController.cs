using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.AWSService;

namespace PersFashApplicationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IAWSService _aWSService;

        public FileController(IAWSService aWSService)
        {
            _aWSService = aWSService;
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile image)
        {
            var result = await _aWSService.UploadFile(image, "persfash-application", null);
            return Ok(result);
        }
    }
}
