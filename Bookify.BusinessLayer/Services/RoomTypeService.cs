using Bookify.BusinessLayer.Contracts;
using Bookify.BusinessLayer.DTOs.RoomDTOs;
using Bookify.BusinessLayer.DTOs.RoomTypeDTOs;
using Bookify.DAL.Entities;
using Bookify.DAL.Repositories;
using Bookify.DAL.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.BusinessLayer.Services
{
    internal class RoomTypeService: IRoomTypeService
    {
        private readonly IUnitOfWork unitOfWork;
        private IGenericRepository<RoomType> roomTypeRepository;
        public RoomTypeService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            roomTypeRepository = unitOfWork.RoomTypes;
        }

        public async Task<RoomTypeDetailsDTO> CreateRoomTypeAsync(RoomTypeCreateDTO roomTypeCreateDTO)
        {
            RoomType roomType = new RoomType
            {
                TypeName =  roomTypeCreateDTO.TypeName,
                Description = roomTypeCreateDTO.Description,
                Capacity = roomTypeCreateDTO.Capacity,
                PricePerNight = roomTypeCreateDTO.PricePerNight,
                BedType = roomTypeCreateDTO.BedType
            };
            await roomTypeRepository.CreateAsync(roomType);
            await unitOfWork.SaveChangesAsync();
            return MapToRoomTypeDetailsDTO(roomType);
        }

        public async Task<RoomTypeDetailsDTO> DeleteRoomTypeAsync(int roomTypeId)
        {
            RoomType? roomType = await roomTypeRepository.Delete(roomTypeId);
            if (roomType == null)
            {
                throw new Exception("Room type not found");
            }
            await unitOfWork.SaveChangesAsync();
            return MapToRoomTypeDetailsDTO(roomType);

        }

        public async Task<RoomTypeDetailsDTO> UpdateRoomTypeAsync(RoomTypeUpdateDTO roomTypeUpdateDTO)
        {
            RoomType? roomType = await roomTypeRepository.GetByIdAsync(roomTypeUpdateDTO.RoomTypeId);
            if (roomType == null)
            {
                throw new Exception($"Room type with ID: {roomTypeUpdateDTO.RoomTypeId} not found");
            }
            roomType.TypeName = roomTypeUpdateDTO.TypeName;
            roomType.Description = roomTypeUpdateDTO.Description;
            roomType.BedType = roomTypeUpdateDTO.BedType;
            roomType.Capacity = roomTypeUpdateDTO.Capacity;
            roomType.PricePerNight = roomTypeUpdateDTO.PricePerNight;
            roomType.BedCount = roomTypeUpdateDTO.BedCount;
            roomType.BathroomCount = roomTypeUpdateDTO.BathroomCount;

            roomTypeRepository.Update(roomType);
            await unitOfWork.SaveChangesAsync();
            return await ViewRoomTypeDetails(roomTypeUpdateDTO.RoomTypeId);
        }
        public async Task<RoomTypeDetailsDTO> ViewRoomTypeDetails(int roomTypeId)
        {
            RoomType? roomType = await roomTypeRepository.GetByIdAsync(roomTypeId);
            if (roomType==null)
            {
                throw new Exception("Room type not found");
            }
            return MapToRoomTypeDetailsDTO(roomType);
        }

        public async Task<List<RoomTypeDetailsDTO>> ViewAllRoomTypes()
        {
            return await roomTypeRepository.GetAllAsQueryable().Select(
                roomType => new RoomTypeDetailsDTO
                {
                    RoomTypeId = roomType.RoomTypeId,
                    TypeName = roomType.TypeName,
                    Description = roomType.Description,
                    Capacity = roomType.Capacity,
                    PricePerNight = roomType.PricePerNight,
                    BedCount = roomType.BedCount,
                    BedType = roomType.BedType,
                    BathroomCount = roomType.BathroomCount
                }
                ).AsNoTracking().ToListAsync(
                );
        }


        public RoomTypeDetailsDTO MapToRoomTypeDetailsDTO(RoomType roomType) 
        {
            return new RoomTypeDetailsDTO
            {
                RoomTypeId = roomType.RoomTypeId,
                TypeName = roomType.TypeName,
                Description = roomType.Description,
                Capacity = roomType.Capacity,
                PricePerNight = roomType.PricePerNight,
                BedCount = roomType.BedCount,
                BedType = roomType.BedType,
                BathroomCount = roomType.BathroomCount
            };
        }

        //public async Task<List<RoomImage>> ExtractImageAsync(List<IFormFile> formFiles, int roomId)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
