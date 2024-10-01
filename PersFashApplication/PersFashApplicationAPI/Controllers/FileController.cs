using BusinessObject.Models.ResultModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.AWSService;
using Services.CloudinaryService;
using System.Net;

namespace PersFashApplicationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IAWSService _aWSService;
        private readonly ICloudinaryService _cloudinaryService;

        public FileController(IAWSService aWSService, ICloudinaryService cloudinaryService)
        {
            _aWSService = aWSService;
            _cloudinaryService = cloudinaryService;
        }

        /// <summary>
        /// For uploading images
        /// </summary>
        [HttpPost]
        [Route("image")]
        public async Task<IActionResult> UploadFileImages(List<IFormFile> images)
        {
            var result = await _aWSService.UploadFilesImages(images, "persfash-application", null);

            var response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Upload images successfully",
                Data = result
            };

            return StatusCode(response.Code, response);
        }

        [HttpPost]
        [Route("cloudinary/image")]
        public async Task<IActionResult> UploadFileImagesCloudinary(List<IFormFile> images)
        {
            var result = await _cloudinaryService.AddPhotos(images);

            var response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Upload images successfully",
                Data = result
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// For uploading other files
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> UploadFile(List<IFormFile> files)
        {
            var result = await _aWSService.UploadFilesImages(files, "persfash-application", null);

            var response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Upload file successfully",
                Data = result
            };

            return StatusCode(response.Code, response);
        }
    }
}
