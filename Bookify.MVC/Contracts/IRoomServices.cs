
using Bookify.MVC.Models.RoomDTOs;

namespace Bookify.MVC.Contracts
{
    public interface IRoomServices
    {
        Task<List<RoomViewDTO>> GetAllRoomsAsync(int? roomTypeId, Status? status);
        Task<RoomViewDTO?> GetRoomByIdAsync(int id);
        Task<RoomDetailsDTO?> GetRoomDetailsAsync(int roomId);
    }
}
