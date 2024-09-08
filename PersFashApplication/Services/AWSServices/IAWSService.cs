using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.AWSService
{
    public interface IAWSService
    {
        public Task<string> UploadFile(IFormFile file, string bucketName, string? prefix);
        public Task<List<string>> UploadFiles(List<IFormFile> files, string bucketName, string? prefix);
        Task DeleteFile(string bucketName, string key);
        public string ExtractS3Key(string url);
    }
}
