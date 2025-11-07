using Bookify.BusinessLayer.Contracts;
using Bookify.BusinessLayer.DTOs.RoomDTOs;
using Bookify.DAL.Repositories;
using Bookify.DAL.UnitOfWork;
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
        public RoomService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            roomRepository = unitOfWork.Rooms;
        }

        public Task<RoomDetailsDTO> CreateRoomAsync(RoomCreateDTO roomCreateDTO)
        {
            throw new NotImplementedException();
        }

        public Task<RoomDetailsDTO> DeleteRoom(int roomId)
        {
            throw new NotImplementedException();
        }

        public Task<RoomDetailsDTO> UpdateRoom(RoomUpdateDTO roomUpdateDTO)
        {
            throw new NotImplementedException();
        }

        public Task<RoomViewDTO> ViewAllRooms()
        {
            throw new NotImplementedException();
        }

        public Task<RoomDetailsDTO> ViewRoomDetails(int roomId)
        {
            throw new NotImplementedException();
        }
    }
}
