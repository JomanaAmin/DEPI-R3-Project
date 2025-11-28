using Bookify.BusinessLayer.DTOs.RoomDTOs;
using Bookify.DAL.Entities;
using Bookify.MVC.Models;

namespace Bookify.MVC.Contracts
{
    public interface IRoomServices
    {
        Task<List<RoomViewDTO>> GetAllRoomsAsync(int? roomTypeId, RoomStatus? status)
    }
}
