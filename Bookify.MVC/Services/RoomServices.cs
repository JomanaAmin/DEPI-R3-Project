using Bookify.DAL.Entities;
using Bookify.MVC.Contracts;
using Bookify.MVC.Models;
using Bookify.MVC.Models.RoomDTOs;
using System.Drawing;
using System.Net.Http;
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
            return response.Rooms;
        }
    }
}
