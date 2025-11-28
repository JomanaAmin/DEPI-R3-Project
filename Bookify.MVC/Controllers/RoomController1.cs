using System.Drawing;
using Bookify.BusinessLayer.Contracts;
using Bookify.DAL.Entities;
using Bookify.MVC.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.MVC.Controllers
{
    public class RoomController1 : Controller
    {
        private readonly IRoomServices RoomServices;
        public RoomController1(IRoomServices roomService)
        {
            this.RoomServices = RoomServices;
        }

        // Accept optional query parameters for filtering
        public async Task<IActionResult> Index(string? roomId, string? floor, string? roomTypeName)
        {
            var rooms = await RoomServices.GetRoomDtos(roomId ?? string.Empty, floor ?? string.Empty, roomTypeName ?? string.Empty);
            return View(rooms); // Pass data to the view
        }
    }
}
