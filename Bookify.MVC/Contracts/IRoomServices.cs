using Bookify.DAL.Entities;
using Bookify.BusinessLayer.DTOs.RoomDTOs;

namespace Bookify.MVC.Contracts
{
    public interface IRoomServices
    {
        Task<List<RoomViewDTO>> GetAllRoomsAsync(int? roomTypeId, RoomStatus? status);
        Task<RoomViewDTO?> GetRoomByIdAsync(int id);
        Task<RoomDetailsDTO?> GetRoomDetailsAsync(int roomId);
    }
}
