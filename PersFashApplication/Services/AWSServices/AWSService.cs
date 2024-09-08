using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using Microsoft.AspNetCore.Http;
using Services.AWSService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagLib;

namespace Services.AWSServices
{
    public class AWSService : IAWSService
    {
        private readonly IAmazonS3 _amazonS3;

        public AWSService(IAmazonS3 amazonS3)
        {
            _amazonS3 = amazonS3;
        }
        public async Task DeleteFile(string bucketName, string key)
        {
            var bucketExist = await AmazonS3Util.DoesS3BucketExistV2Async(_amazonS3, bucketName);
            if (!bucketExist) throw new Exception("Bucket does not exist");
            await _amazonS3.DeleteObjectAsync(bucketName, key);
        }

        public string ExtractS3Key(string url)
        {
            var parts = url.Split(new[] { ".com/" }, StringSplitOptions.None);
            return parts.Length > 1 ? parts[1] : string.Empty;
        }

        public async Task<string> UploadFile(IFormFile file, string bucketName, string? prefix)
        {

            //TimeSpan duration;

            //using (var stream = file.OpenReadStream())
            //{
            //    var tfile = TagLib.File.Create(new TagLib.StreamFileAbstraction(file.FileName, stream, stream));
            //    duration = tfile.Properties.Duration;
            //}

                var bucketExist = await AmazonS3Util.DoesS3BucketExistV2Async(_amazonS3, bucketName);
            if (!bucketExist) throw new Exception("Bucket does not exist");

            var request = new PutObjectRequest
            {
                BucketName = bucketName,
                Key = string.IsNullOrEmpty(prefix) ? file.FileName : $"{prefix?.Trim('/')}/{file.Name}",
                InputStream = file.OpenReadStream()
            };

            request.Metadata.Add("Content-Type", file.ContentType);
            var uploadResult = await _amazonS3.PutObjectAsync(request);


            var metadata = await _amazonS3.GetObjectMetadataAsync(new GetObjectMetadataRequest
            {
                BucketName = bucketName,
                Key = request.Key
            });

            if (metadata == null) throw new Exception("Object metadata does not exist");

            string url = $"https://{bucketName}.s3.ap-southeast-2.amazonaws.com/{request.Key}";

            return url;
        }

        public async Task<List<string>> UploadFiles(List<IFormFile> files, string bucketName, string? prefix)
        {
            List<string> imageURLs = [];

            var bucketExist = await AmazonS3Util.DoesS3BucketExistV2Async(_amazonS3, bucketName);
            if (!bucketExist) throw new Exception("Bucket does not exist");

            foreach (var file in files)
            {
                var request = new PutObjectRequest
                {
                    BucketName = bucketName,
                    Key = string.IsNullOrEmpty(prefix) ? file.FileName : $"{prefix?.Trim('/')}/{file.Name}",
                    InputStream = file.OpenReadStream()
                };

                request.Metadata.Add("Content-Type", file.ContentType);
                var uploadResult = await _amazonS3.PutObjectAsync(request);


                var metadata = await _amazonS3.GetObjectMetadataAsync(new GetObjectMetadataRequest
                {
                    BucketName = bucketName,
                    Key = request.Key
                });

                if (metadata == null) throw new Exception("Object metadata does not exist");

                string url = $"https://{bucketName}.s3.ap-southeast-2.amazonaws.com/{request.Key}";

                imageURLs.Add(url);
            }

            return imageURLs;
        }
    }
}
