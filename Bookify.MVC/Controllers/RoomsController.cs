using Bookify.MVC.Contracts; // IRoomServices
using Microsoft.AspNetCore.Mvc;

namespace Bookify.MVC.Controllers
{
    public class RoomsController : Controller
    {
        private readonly IRoomServices _roomServices;
        public RoomsController(IRoomServices roomServices)
        {
            _roomServices = roomServices;
        }

        // /Rooms?roomTypeId=&status=
        [HttpGet]
        public async Task<IActionResult> Index(int? roomTypeId, Status? status)
        {
            var rooms = await _roomServices.GetAllRoomsAsync(roomTypeId, status);
            return View(rooms);
        }
    }
}
