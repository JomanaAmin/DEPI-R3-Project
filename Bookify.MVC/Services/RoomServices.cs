using System.Web;
using Bookify.MVC.Contracts;
using Bookify.MVC.Models;

namespace Bookify.MVC.Services
{
    public class RoomServices : IRoomServices
    {
        private readonly HttpClient _httpClient;
        public RoomServices(HttpClient httpClient) { _httpClient = httpClient; }
        public async Task<List<RoomDto>> GetRoomDtos(string roomId, string Floor, string RoomTypeName)
        {
            // Build query string if filters are provided
            var baseUri = _httpClient.BaseAddress?.ToString() ?? "/";
            var builder = new UriBuilder(new Uri(new Uri(baseUri), "room"));
            var query = HttpUtility.ParseQueryString(builder.Query);
            if (!string.IsNullOrWhiteSpace(roomId)) query["roomId"] = roomId;
            if (!string.IsNullOrWhiteSpace(Floor)) query["floor"] = Floor;
            if (!string.IsNullOrWhiteSpace(RoomTypeName)) query["roomTypeName"] = RoomTypeName;
            builder.Query = query.ToString();

            // Expect API to return a list of RoomDto
            var rooms = await _httpClient.GetFromJsonAsync<List<RoomDto>>(builder.Uri);
            return rooms ?? new List<RoomDto>();
        }
    }
}
