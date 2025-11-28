using Bookify.DAL.Entities;
using Bookify.MVC.Models;
using Bookify.MVC.Models.RoomDTOs;

namespace Bookify.MVC.Contracts
{
    public interface IRoomServices
    {
        Task<List<RoomViewDTO>> GetAllRoomsAsync(int? roomTypeId, RoomStatus? status);
        Task<RoomViewDTO?> GetRoomByIdAsync(int id);
    }
}
