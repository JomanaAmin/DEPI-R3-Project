using Bookify.MVC.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.MVC.Controllers
{
    public class RoomController : Controller
    {
        private readonly IRoomServices RoomServices;
        public RoomController(IRoomServices roomService)
        {
            this.RoomServices = roomService;
        }

        // take optional query 
        public async Task<IActionResult> Index(int? roomTypeId, Status? status)
        {
            var rooms = await RoomServices.GetAllRoomsAsync(roomTypeId, status);
            return View(rooms); // Pass data 
        }
    }
}
