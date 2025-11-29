using Bookify.MVC.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.MVC.Controllers
{
    // Server-rendered replacement for static /app/room.html when given ?id=
    public class RoomDetailsController : Controller
    {
        private readonly IRoomServices _roomServices;
        public RoomDetailsController(IRoomServices roomServices)
        {
            _roomServices = roomServices;
        }

        // GET /RoomDetails?id=123 mapped to view
        [HttpGet]
        public async Task<IActionResult> Index(int id)
        {
            var room = await _roomServices.GetRoomDetailsAsync(id);
            if (room == null) return NotFound();
            return View(room);
        }
    }
}