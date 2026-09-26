using Clinck.Domain.Consts;
using Clinck.Web.Services.Contract;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Clinck.Web.Services.Implementation
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _webHost;
        private readonly List<string> _allowedExtentions = new List<string>() { ".jpg", ".png", ".jpeg" };
        private readonly int _maxAllowSize = 2 * 1024 * 1024;

        public ImageService(IWebHostEnvironment webHost)
        {
            _webHost = webHost;
        }

        public async Task<(bool IsUploaded, string? ErrorMessage)> UploadAsync(IFormFile image, string imageName, string folderPath, bool hasThumbnail)
        {
            var extension = Path.GetExtension(image.FileName).ToLower();
            if (!_allowedExtentions.Contains(extension))
            {
                return (IsUploaded: false, ErrorMessage: Errors.NotAllowedExtention);
            }

            if (image.Length > _maxAllowSize)
            {
                return (IsUploaded: false, ErrorMessage: Errors.MaxSize);
            }

            var webRoot = _webHost.WebRootPath ?? Path.Combine(_webHost.ContentRootPath, "wwwroot");
            var cleanFolderPath = folderPath.TrimStart('/', '\\');
            var directoryPath = Path.Combine(webRoot, cleanFolderPath);

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var path = Path.Combine(directoryPath, imageName);
            using (var stream = System.IO.File.Create(path))
            {
                await image.CopyToAsync(stream);
            }

            if (hasThumbnail)
            {
                var thumbDirectoryPath = Path.Combine(directoryPath, "Thumb");
                if (!Directory.Exists(thumbDirectoryPath))
                {
                    Directory.CreateDirectory(thumbDirectoryPath);
                }

                using var loadedImage = SixLabors.ImageSharp.Image.Load(image.OpenReadStream());
                var ratio = (float)loadedImage.Width / 200;
                var height = loadedImage.Height / ratio;
                loadedImage.Mutate(i => i.Resize(width: 200, height: (int)height));

                var thumbPath = Path.Combine(thumbDirectoryPath, imageName);
                await loadedImage.SaveAsync(thumbPath);
            }

            return (IsUploaded: true, ErrorMessage: null);
        }

        public void Delete(string imagePath, string? thumnailPath = null)
        {
            var webRoot = _webHost.WebRootPath ?? Path.Combine(_webHost.ContentRootPath, "wwwroot");

            if (!string.IsNullOrEmpty(imagePath))
            {
                var oldPathImage = Path.Combine(webRoot, imagePath.TrimStart('/', '\\'));
                if (File.Exists(oldPathImage))
                {
                    File.Delete(oldPathImage);
                }
            }

            if (!string.IsNullOrEmpty(thumnailPath))
            {
                var oldPathThum = Path.Combine(webRoot, thumnailPath.TrimStart('/', '\\'));
                if (File.Exists(oldPathThum))
                {
                    File.Delete(oldPathThum);
                }
            }
        }
    }
}
