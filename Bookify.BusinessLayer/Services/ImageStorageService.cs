using Bookify.BusinessLayer.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting; 
namespace Bookify.BusinessLayer.Services
{
    internal class ImageStorageService : IImageStorageService
    {

        public async Task<string> SaveImagesAsync(IFormFile image, int roomId)
        {
            string imageName = $"{Guid.NewGuid()}_{image.FileName}_{roomId}";
            string filePath = Path.Combine("wwwroot", "images", imageName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }
            return $"/images/{imageName}";
        }
        public async Task DeleteImageAsync(string imageUrl)
        {
            string filePath = Path.Combine("wwwroot", imageUrl.TrimStart('/'));
            if (File.Exists(filePath))
            {
                await Task.Run(() => File.Delete(filePath));
            }
        }

    }
}
