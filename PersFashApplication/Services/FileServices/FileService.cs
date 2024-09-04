using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.FileServices
{
    public class FileService : IFileService
    {
        public void CheckImageFile(IFormFile image_path)
        {
            var permittedImageExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var imageFileExtension = Path.GetExtension(image_path.FileName).ToLowerInvariant();

            if (string.IsNullOrEmpty(imageFileExtension) || !permittedImageExtensions.Contains(imageFileExtension))
            {
                throw new Exception("Invalid image file. Please upload an image file.");
            }
        }

        public void CheckMusicFile(IFormFile music_path)
        {
            var permittedAudioExtensions = new[] { ".mp3", ".wav", ".aac", ".flac" };
            var musicFileExtension = Path.GetExtension(music_path.FileName).ToLowerInvariant();

            if (string.IsNullOrEmpty(musicFileExtension) || !permittedAudioExtensions.Contains(musicFileExtension))
            {
                throw new Exception("Invalid music file, please upload an audio file");
            }
        }

        public TimeOnly GetDurationOfMusicPath(IFormFile music_path)
        {
            TimeOnly finalDuration;

            using (var stream = music_path.OpenReadStream())
            {
                var tfile = TagLib.File.Create(new TagLib.StreamFileAbstraction(music_path.FileName, stream, stream));
                TimeSpan duration = tfile.Properties.Duration;

                var roundedDuration = TimeSpan.FromSeconds(Math.Ceiling(duration.TotalSeconds));

                // Format the duration as "hh:mm:ss"
                finalDuration = new TimeOnly(roundedDuration.Hours, roundedDuration.Minutes, roundedDuration.Seconds);
            }

            return finalDuration;
        }
    }
}
