using Bookify.MVC.Models.RoomTypeDTOs;

namespace Bookify.MVC.Services
{
    public class RoomTypeService
    {
        private readonly HttpClient httpClient;
        public RoomTypeService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
            //httpClient.BaseAddress = new Uri("https://localhost:7242/api/"); 
        }

        public async Task<List<RoomTypeViewDTO>> GetRoomTypes() 
        {
            var response = await httpClient.GetAsync("RoomType");
            if (response.IsSuccessStatusCode) 
            {
                var data = await response.Content.ReadFromJsonAsync<List<RoomTypeViewDTO>>();
                return data.Select( roomType=>
                new RoomTypeViewDTO {
                    RoomTypeId=roomType.RoomTypeId,
                    TypeName=roomType.TypeName,
                    Description=roomType.Description,
                    Capacity=roomType.Capacity,
                    PricePerNight=roomType.PricePerNight,
                    BedCount=roomType.BedCount,
                    BedType=roomType.BedType,
                    BathroomCount=roomType.BathroomCount
                }
                    ).ToList();
            }
            return null;
        }
        public async Task<RoomTypeViewDTO> GetRoomTypeById(int id) 
        {
            var response = await httpClient.GetAsync($"RoomType/{id}");
            if (response.IsSuccessStatusCode) 
            {
                var data = await response.Content.ReadFromJsonAsync<RoomTypeViewDTO>();
                return data;
            }
            return null;
        }
    }
}
