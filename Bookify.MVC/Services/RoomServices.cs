using Bookify.DAL.Entities;
using Bookify.MVC.Contracts;
using Bookify.MVC.Models;
using Bookify.MVC.Models.RoomDTOs;
using System.Drawing;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using System.Web;

namespace Bookify.MVC.Services
{
    public class RoomServices : IRoomServices
    {
        private readonly HttpClient _httpClient;
        public RoomServices(HttpClient httpClient) { _httpClient = httpClient; }
        public async Task<List<RoomViewDTO>> GetAllRoomsAsync(int? roomTypeId, RoomStatus? status) 
        {
            var baseUri = _httpClient.BaseAddress?.ToString() ?? "/";
            var builder = new UriBuilder(new Uri(new Uri(baseUri), "room"));
            var query = HttpUtility.ParseQueryString(builder.Query);
            if (roomTypeId!=null) query["roomTypeId"] = roomTypeId.ToString();
            if (status!=null) query["status"] = status.ToString(); 
            var response = await _httpClient.GetFromJsonAsync<AllRoomsDTO>("Room");
            if(response?.Rooms==null)
                return new List<RoomViewDTO>();
            return response.Rooms.Select(
                room=> new RoomViewDTO
                {
                    RoomId=room.RoomId,
                    RoomTypeId=room.RoomTypeId,
                    RoomTypeName=room.RoomTypeName,
                    PricePerNight=room.PricePerNight,
                    Status=room.Status,
                    ThumbnailImage=room.ThumbnailImage
                }
                ).ToList();
        }
        public async Task<RoomViewDTO?> GetRoomByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"Room/{id}");

            // explicit handling so calling code (controller) can catch specific problems
            if (response.StatusCode == HttpStatusCode.NotFound)
                throw new KeyNotFoundException($"Room with id {id} not found.");

            //if (response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.Forbidden)
            //    throw new AuthenticationException("Not authorized to view this room.");

            response.EnsureSuccessStatusCode();

            var room = await response.Content.ReadFromJsonAsync<RoomViewDTO>();
            return room;
        }
    }
}
