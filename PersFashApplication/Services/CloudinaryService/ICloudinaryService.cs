using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.CloudinaryService
{
    public interface ICloudinaryService
    {
        Task<RawUploadResult> AddAudio(IFormFile file);
        Task<ImageUploadResult> AddPhoto(IFormFile file);
        Task<List<string>> AddPhotos(List<IFormFile> file);
        Task<DeletionResult> DeleteFile(string publicId);
    }
}
