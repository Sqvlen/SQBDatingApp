using CloudinaryDotNet.Actions;

namespace WebAPI.Interfaces
{
    public interface IPhotoService
    {
        public Task<ImageUploadResult> AddPhotoAsync(IFormFile formFile);

        public Task<DeletionResult> DeletePhotoAsync(string publicId);
    }
}
