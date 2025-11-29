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
    internal class RoomService : IRoomService
    {
        private readonly IUnitOfWork unitOfWork;
        private IRoomRepository roomRepository;
        private IGenericRepository<RoomType> roomTypeRepository;
        private IRoomImageRepository roomImageRepository;
        private IImageStorageService imageStorageService;
        public RoomService(IUnitOfWork unitOfWork, IImageStorageService imageStorageService)
        {
            this.unitOfWork = unitOfWork;
            roomRepository = unitOfWork.Rooms;
            roomTypeRepository = unitOfWork.RoomTypes;
            roomImageRepository = unitOfWork.RoomImages;
            this.imageStorageService = imageStorageService;
        }

        public async Task<RoomDetailsDTO> CreateRoomAsync(RoomCreateDTO roomCreateDTO)
        {
            RoomType? roomType = await roomTypeRepository.GetByIdAsync(roomCreateDTO.RoomTypeId);
            if (roomType == null) 
            {
                ExceptionFactory.CreateRoomNotFoundException();
            }
            Room room = new Room 
            {
                RoomTypeId = roomCreateDTO.RoomTypeId,
                Floor = roomCreateDTO.Floor,
                BuildingNumber = roomCreateDTO.BuildingNumber,
                Status = roomCreateDTO.Status,
               
            };
            await roomRepository.CreateAsync(room);
            await unitOfWork.SaveChangesAsync();
            int roomId = room.RoomId;

            List<RoomImage> images = new List<RoomImage>();
            images= await ExtractImageAsync(roomCreateDTO.Images, roomId);
            room.RoomImages= images;
            return MapToRoomDetailsDTO(room);

        }
        public async Task DeleteRoomAsync(int roomId)
        {
            Room? room = await roomRepository.Delete(roomId);
            if (room == null) ExceptionFactory.CreateRoomNotFoundException();
            await unitOfWork.SaveChangesAsync();
        }
        //public async Task<RoomDetailsDTO> DeleteRoomAsync(int roomId)
        //{
        //    Room? room = await roomRepository.Delete(roomId);
        //    if(room == null) throw new Exception("Room not found");
        //    await unitOfWork.SaveChangesAsync();
        //    return MapToRoomDetailsDTO(room);
        //}

        public async Task<RoomDetailsDTO> UpdateRoomAsync(RoomUpdateDTO roomUpdateDTO)
        {
            Room room= await roomRepository.GetByIdAsync(roomUpdateDTO.RoomId);
            if (room == null) 
            {
                ExceptionFactory.CreateRoomNotFoundException();
            }
            room.RoomTypeId = roomUpdateDTO.RoomTypeId;
            room.Floor= roomUpdateDTO.Floor;
            room.BuildingNumber = roomUpdateDTO.BuildingNumber;
            room.Status = roomUpdateDTO.Status;
            roomRepository.Update(new Room 
            { 
                RoomId= roomUpdateDTO.RoomId,
                RoomTypeId = roomUpdateDTO.RoomTypeId,
                Floor = roomUpdateDTO.Floor,
                BuildingNumber= roomUpdateDTO.BuildingNumber,
                Status = roomUpdateDTO.Status
            });
            if ((roomUpdateDTO.Images != null && roomUpdateDTO.Images.Any() ))
            {
                List<RoomImage> images =  await ExtractImageAsync(roomUpdateDTO.Images, roomUpdateDTO.RoomId);
                await roomImageRepository.AddImagesRangeAsync(images);
                   
            }
            if (roomUpdateDTO.ImagesToDelete != null && roomUpdateDTO.ImagesToDelete.Any())
            {
                List<RoomImage> images = await roomImageRepository.DeleteImagesRangeAsync(roomUpdateDTO.ImagesToDelete);
                foreach (var img in images)
                {
                    await imageStorageService.DeleteImageAsync(img.ImageUrl);
                }

            }

            await unitOfWork.SaveChangesAsync();
            return await ViewRoomDetails(roomUpdateDTO.RoomId);
        }

        public async Task<List<RoomViewDTO>> ViewAllRooms(int? roomTypeId, RoomStatus? status)
        {
            var query = roomRepository.GetAllAsQueryable();
            if (roomTypeId != null)
            {
                query=query.Where(r => r.RoomTypeId == roomTypeId);
            }
            if (status!=null) 
            {
                query=query.Where(r => r.Status == status);
            }
            List<RoomViewDTO> rooms = await query
             
                .Select(
                r => new RoomViewDTO
                {
                    RoomId = r.RoomId,
                    RoomTypeId = r.RoomTypeId,
                    RoomTypeName = r.RoomType.TypeName,
                    PricePerNight = r.RoomType.PricePerNight,
                    Status = r.Status,
                    ThumbnailImage = r.RoomImages.Select(
                        img => img.ImageUrl
                        ).FirstOrDefault()

                }
                ).AsNoTracking().ToListAsync();
            return rooms;
        }

       

        public async Task<RoomDetailsDTO> ViewRoomDetails(int roomId)
        {
            RoomDetailsDTO? room = await roomRepository.GetAllAsQueryable().Select(
                r => new RoomDetailsDTO
                {
                    RoomId = r.RoomId,
                    //RoomTypeId = r.RoomTypeId,
                    //RoomTypeName = r.RoomType.TypeName,
                    //PricePerNight = r.RoomType.PricePerNight,
                    Floor = r.Floor,
                    BuildingNumber = r.BuildingNumber,
                    Status = r.Status,
                    RoomTypeDetails = new RoomTypeDetailsDTO 
                    {
                        RoomTypeId = r.RoomType.RoomTypeId,
                        TypeName = r.RoomType.TypeName,
                        Description = r.RoomType.Description,
                        Capacity = r.RoomType.Capacity,
                        PricePerNight = r.RoomType.PricePerNight,
                        BedCount = r.RoomType.BedCount,
                        BedType = r.RoomType.BedType,
                        BathroomCount = r.RoomType.BathroomCount
                    },
                    Images = r.RoomImages.Select(
                        img => img.ImageUrl
                        ).ToList()

                }
                ).SingleOrDefaultAsync(r => r.RoomId == roomId);
            if (room == null) ExceptionFactory.CreateRoomNotFoundException();
            return room;

        }


        public RoomDetailsDTO MapToRoomDetailsDTO(Room room)
        {
            return new RoomDetailsDTO
            {
                RoomId = room.RoomId,
                //RoomTypeId = room.RoomTypeId,
                //RoomTypeName = room.RoomType.TypeName,
                //PricePerNight = room.RoomType.PricePerNight,
                Floor = room.Floor,
                BuildingNumber = room.BuildingNumber,
                Status = room.Status,
                Images = room.RoomImages.Select(img => img.ImageUrl).ToList(),
                RoomTypeDetails= new RoomTypeDetailsDTO
                {
                    RoomTypeId = room.RoomType.RoomTypeId,
                    TypeName = room.RoomType.TypeName,
                    Description = room.RoomType.Description,
                    Capacity = room.RoomType.Capacity,
                    PricePerNight = room.RoomType.PricePerNight,
                    BedCount = room.RoomType.BedCount,
                    BedType = room.RoomType.BedType,
                    BathroomCount = room.RoomType.BathroomCount
                }
            };
        }
        public RoomViewDTO MapToRoomViewDTO(Room room)
        {
            return new RoomViewDTO
            {
                RoomId = room.RoomId,
                RoomTypeId = room.RoomTypeId,
                RoomTypeName = room.RoomType.TypeName,
                PricePerNight = room.RoomType.PricePerNight,
                Status = room.Status,
                ThumbnailImage = room.RoomImages.Select(img => img.ImageUrl).FirstOrDefault()
            };
        }
 
        public async Task<List<RoomImage>> ExtractImageAsync(List<IFormFile> formFiles, int roomId)
        {
            List<RoomImage> roomImages = new List<RoomImage>();
            foreach (IFormFile formFile in formFiles)
            {
                RoomImage roomImage = new RoomImage
                {
                    RoomId = roomId,
                    ImageUrl = await imageStorageService.SaveImagesAsync(formFile, roomId)
                };
                await roomImageRepository.CreateAsync(roomImage);
                roomImages.Add(roomImage);
            }
            await unitOfWork.SaveChangesAsync();

            return roomImages;


        }
    }
}
