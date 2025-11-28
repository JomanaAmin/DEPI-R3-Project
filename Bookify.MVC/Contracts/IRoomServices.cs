using Bookify.BusinessLayer.DTOs.RoomDTOs;
using Bookify.MVC.Models;

namespace Bookify.MVC.Contracts
{
    public interface IRoomServices
    {
        Task<List<RoomDto>> GetRoomDtos(string roomId, string Floor, string RoomTypeName);
    }
}
