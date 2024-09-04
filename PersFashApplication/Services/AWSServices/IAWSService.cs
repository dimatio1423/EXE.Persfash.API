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
        Task DeleteFile(string bucketName, string key);
    }
}
