using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.FileServices
{
    public interface IFileService
    {
        void CheckMusicFile(IFormFile music_path);
        void CheckImageFile(IFormFile image_path);
        TimeOnly GetDurationOfMusicPath(IFormFile music_path);
    }
}
