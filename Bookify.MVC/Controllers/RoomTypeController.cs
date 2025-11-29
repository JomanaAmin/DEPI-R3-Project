using Bookify.MVC.Models.RoomTypeDTOs;
using Bookify.MVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.MVC.Controllers
{
    public class RoomTypeController : Controller
    {
        private readonly RoomTypeService roomTypeService;
        public RoomTypeController(RoomTypeService roomTypeService)
        {
            this.roomTypeService = roomTypeService;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<RoomTypeViewDTO>? roomTypes = await roomTypeService.GetRoomTypes();
            if(roomTypes == null)
            {
                return NotFound();
            }
            return Json(roomTypes);
        }
        public async Task<IActionResult> SingleType (int id)
        {
            RoomTypeViewDTO? roomType = await roomTypeService.GetRoomTypeById(id);
            if(roomType == null)
            {
                return NotFound();
            }
            return Json(roomType);
        }
    }
}
