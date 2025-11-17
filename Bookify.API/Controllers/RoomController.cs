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
            return Ok(response);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllRooms()
        {
            var response = await roomService.ViewAllRooms();
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
        { return Ok(await roomService.DeleteRoomAsync(id)); }
    }
}
