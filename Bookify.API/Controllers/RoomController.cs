using Bookify.BusinessLayer.Contracts;
using Bookify.BusinessLayer.DTOs.RoomDTOs;
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
            var response=await roomService.ViewRoomDetails(id);
            //return Ok($"room {id}!");
            return Ok(response);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllRooms()
        {
            return Ok("All rooms!");
        }
        [HttpPost]
        public async Task<IActionResult> CreateRoom(RoomCreateDTO roomCreateDTO)
        { 
            var response=await roomService.CreateRoomAsync(roomCreateDTO);
            return Ok(response); 
            //return Ok("Room created!"); 
        }

        [HttpPut]
        public async Task<IActionResult> UpdateRoom()
        { return Ok("Room updated!"); }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoom(int id)
        { return Ok($"Room {id} deleted!"); }
    }
}
