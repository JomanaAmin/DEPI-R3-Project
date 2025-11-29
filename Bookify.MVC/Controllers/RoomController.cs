using System.Drawing;
using Bookify.BusinessLayer.Contracts;
using Bookify.DAL.Entities;
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

        // Accept optional query parameters for filtering
        public async Task<IActionResult> Index(int? roomTypeId, RoomStatus? status)
        {
            var rooms = await RoomServices.GetAllRoomsAsync(roomTypeId, status);
            return View(rooms); // Pass data to the view
        }
    }
}
