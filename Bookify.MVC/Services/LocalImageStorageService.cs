namespace Bookify.MVC.Services
{
    public class LocalImageStorageService 
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        // Dependency injection to get information about the web hosting environment
        public LocalImageStorageService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<string> SaveImagesAsync(IFormFile image, int roomId)
        {
            // Get the absolute path to the wwwroot folder
            string wwwRootPath = _webHostEnvironment.WebRootPath;

            // Define the images subfolder path
            string imageFolderPath = Path.Combine(wwwRootPath, "images");

            // Ensure the directory exists
            if (!Directory.Exists(imageFolderPath))
            {
                Directory.CreateDirectory(imageFolderPath);
            }

            string imageName = $"{Guid.NewGuid()}_{image.FileName}_{roomId}";

            // Combine the absolute path to the final file location
            string filePath = Path.Combine(imageFolderPath, imageName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            // Return the relative URL for database storage
            return $"/images/{imageName}";
        }

        public async Task DeleteImageAsync(string imageUrl)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;

            // Construct the absolute file path, stripping the leading '/' from the URL
            string filePath = Path.Combine(wwwRootPath, imageUrl.TrimStart('/'));

            if (File.Exists(filePath))
            {
                await Task.Run(() => File.Delete(filePath));
            }
        }
    }
}
