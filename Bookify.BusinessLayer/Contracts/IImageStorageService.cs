using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.BusinessLayer.Contracts
{
    public interface IImageStorageService
    {
        Task<string> SaveImagesAsync(IFormFile image, int roomId);
        Task DeleteImageAsync(string imageUrl);
    }
}
