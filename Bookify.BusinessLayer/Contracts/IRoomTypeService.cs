using Bookify.BusinessLayer.DTOs.RoomDTOs;
using Bookify.BusinessLayer.DTOs.RoomTypeDTOs;
using Bookify.DAL.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.BusinessLayer.Contracts
{
    public interface IRoomTypeService
    {
        Task<RoomTypeDetailsDTO> CreateRoomTypeAsync(RoomTypeCreateDTO roomTypeCreateDTO);
        Task<RoomTypeDetailsDTO> ViewRoomTypeDetails(int roomTypeId);
        Task<List<RoomTypeDetailsDTO>> ViewAllRoomTypes();
        Task<RoomTypeDetailsDTO> UpdateRoomTypeAsync(RoomTypeUpdateDTO roomTypeUpdateDTO);
        Task<RoomTypeDetailsDTO> DeleteRoomTypeAsync(int roomTypeId);
        //Task<List<RoomImage>> ExtractImageAsync(List<IFormFile> formFiles, int roomId);
    }
}
