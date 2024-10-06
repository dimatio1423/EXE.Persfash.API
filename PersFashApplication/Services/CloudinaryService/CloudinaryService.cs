using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Services.FileServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.CloudinaryService
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;
        private readonly IFileService _fileService;


        public CloudinaryService(IOptions<CloudinarySettings> config, IFileService fileService)
        {
            var acc = new Account(
               config.Value.CloudName,
               config.Value.ApiKey,
               config.Value.ApiSecret
               );
            _cloudinary = new Cloudinary(acc);
            _fileService = fileService;
        }
        public async Task<RawUploadResult> AddAudio(IFormFile file)
        {
            var uploadResult = new RawUploadResult();
            //double? duration = null;

            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new RawUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                };
                uploadResult = await _cloudinary.UploadAsync(uploadParams);
                //duration = uploadResult.;
            }

            //duration = uploadResult

            return uploadResult;
        }

        public async Task<ImageUploadResult> AddPhoto(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();
            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face")
                };
                uploadResult = await _cloudinary.UploadAsync(uploadParams);
            }
            return uploadResult;
        }

        public async Task<List<string>> AddPhotos(List<IFormFile> file)
        {
            List<string> imageURLs = [];

            foreach (var item in file)
            {
                _fileService.CheckImageFile(item);
            }

            foreach (var item in file)
            {
                var uploadResult = new ImageUploadResult();
                if (item.Length > 0)
                {
                    using var stream = item.OpenReadStream();
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(item.FileName, stream),
                        Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face")
                    };
                    uploadResult = await _cloudinary.UploadAsync(uploadParams);
                }

                imageURLs.Add(uploadResult.Url.ToString());
            }

            return imageURLs;
        }

        public async Task<DeletionResult> DeleteFile(string publicId)
        {
            var deleteParams = new DeletionParams(publicId)
            {
                ResourceType = ResourceType.Video  // Specify that the resource type is video
            };
            var result = await _cloudinary.DestroyAsync(deleteParams);

            return result;
        }
    }
}
