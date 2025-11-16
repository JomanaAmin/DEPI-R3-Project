using Bookify.BusinessLayer.DTOs.RoomDTOs;
using Bookify.DAL.Entities;
using Microsoft.AspNetCore.Http;
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
        Task<List<RoomViewDTO>> ViewAllRooms();
        Task<RoomDetailsDTO> UpdateRoomAsync(RoomUpdateDTO roomUpdateDTO);
        Task<RoomDetailsDTO> DeleteRoomAsync(int roomId);
        Task<List<RoomImage>> ExtractImageAsync(List<IFormFile> formFiles, int roomId);


    }
}
