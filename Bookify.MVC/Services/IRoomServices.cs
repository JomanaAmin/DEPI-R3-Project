using Bookify.BusinessLayer.DTOs.RoomDTOs;
using Bookify.MVC.Models;

namespace Bookify.MVC.Services
{
    public interface IRoomServices
    {
        Task<List<RoomDto>> GetRoomDtos(string roomId, String Floor, string RoomTypeName);
    }
}
