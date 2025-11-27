using Bookify.BusinessLayer.Contracts;
using Bookify.BusinessLayer.DTOs.RoomDTOs;
using Bookify.DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService roomService;
        public RoomController(IRoomService roomService)
        {
            this.roomService = roomService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoom(int id)
        {
            var response = await roomService.ViewRoomDetails(id);
            return Ok(response);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllRooms(int? roomTypeId, RoomStatus? status)
        {
            var response = await roomService.ViewAllRooms(roomTypeId,status);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRoom(RoomCreateDTO roomCreateDTO)
        { 
            var response=await roomService.CreateRoomAsync(roomCreateDTO);
            return Ok(response); 
        }

        [HttpPut]
        public async Task<IActionResult> UpdateRoom(RoomUpdateDTO roomUpdateDTO)
        { 
            return Ok(await roomService.UpdateRoomAsync(roomUpdateDTO)); 
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            await roomService.DeleteRoomAsync(id);
            return Ok(); 
        }
    }
}
