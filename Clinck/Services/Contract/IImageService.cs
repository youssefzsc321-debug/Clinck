namespace Clinck.Web.Services.Contract
{
    public interface IImageService
    {
        Task<(bool IsUploaded, string? ErrorMessage)> UploadAsync(IFormFile image, string imagName, string folderPath, bool hasThumbnail);


        void Delete(string imagePath, string? thumnailPath = null);
    }
}
