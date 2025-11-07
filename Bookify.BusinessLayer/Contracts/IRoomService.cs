using Bookify.BusinessLayer.DTOs.RoomDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.BusinessLayer.Contracts
{
    public interface IRoomService
    {
        Task<RoomDetailsDTO> CreateRoomAsync(RoomCreateDTO roomCreateDTO);
        Task<RoomDetailsDTO> ViewRoomDetails (int roomId);
        Task<RoomViewDTO> ViewAllRooms();
        Task<RoomDetailsDTO> UpdateRoom(RoomUpdateDTO roomUpdateDTO);
        Task<RoomDetailsDTO> DeleteRoom(int roomId);

    }
}
