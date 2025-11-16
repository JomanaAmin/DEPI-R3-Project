using Bookify.BusinessLayer.Contracts;
using Bookify.BusinessLayer.DTOs.RoomDTOs;
using Bookify.BusinessLayer.DTOs.RoomTypeDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomTypeController : ControllerBase
    {
        private readonly IRoomTypeService roomTypeService;
        public RoomTypeController(IRoomTypeService roomTypeService)
        {
            this.roomTypeService = roomTypeService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoomType(int id)
        {
            var response = await roomTypeService.ViewRoomTypeDetails(id);
            //return Ok($"room {id}!");
            return Ok(response);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllRoomTypes()
        {
            var response = await roomTypeService.ViewAllRoomTypes();
            return Ok(response);
            //return Ok("All rooms!");
        }
        [HttpPost]
        public async Task<IActionResult> CreateRoomType(RoomTypeCreateDTO roomTypeCreateDTO)
        {
            var response = await roomTypeService.CreateRoomTypeAsync(roomTypeCreateDTO);
            return Ok(response);
            //return Ok("Room created!"); 
        }

        [HttpPut]
        public async Task<IActionResult> UpdateRoomType(RoomTypeUpdateDTO roomTypeUpdateDTO)
        {
            var response = await roomTypeService.UpdateRoomTypeAsync(roomTypeUpdateDTO);
            return Ok(response); 
            //return Ok("Room updated!"); 
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoomType(int id)
        {
            return Ok(await roomTypeService.DeleteRoomTypeAsync(id)); 
            //return Ok($"Room {id} deleted!"); 
        }
    }
}

