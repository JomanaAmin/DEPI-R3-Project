using Bookify.DAL.Entities;
using Bookify.MVC.Contracts;
using Bookify.BusinessLayer.DTOs.RoomDTOs;
using Microsoft.AspNetCore.WebUtilities;
using System.Net;
using System.Net.Http.Json;

namespace Bookify.MVC.Services
{
    public class RoomServices : IRoomServices
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<RoomServices> _logger;

        public RoomServices(HttpClient httpClient, ILogger<RoomServices> logger) 
        { 
            _httpClient = httpClient;
            _logger = logger;
        }

        // Fetch all rooms with optional filters
        public async Task<List<RoomViewDTO>> GetAllRoomsAsync(int? roomTypeId, RoomStatus? status)
        {
            var queryParams = new Dictionary<string, string>();
            if (roomTypeId.HasValue) queryParams["roomTypeId"] = roomTypeId.Value.ToString();
            if (status.HasValue) queryParams["status"] = status.Value.ToString();

            var relativeUrl = queryParams.Count == 0
                ? "room"
                : QueryHelpers.AddQueryString("room", queryParams);

            try
            {
                _logger.LogInformation($"Fetching rooms from: {_httpClient.BaseAddress}{relativeUrl}");
                
                var resp = await _httpClient.GetAsync(relativeUrl);
                
                _logger.LogInformation($"API Response Status: {resp.StatusCode}");

                if (!resp.IsSuccessStatusCode)
                {
                    _logger.LogWarning($"API returned non-success status: {resp.StatusCode}");
                    return new List<RoomViewDTO>();
                }

                var rooms = await resp.Content.ReadFromJsonAsync<List<RoomViewDTO>>();
                _logger.LogInformation($"Successfully fetched {rooms?.Count ?? 0} rooms");
                return rooms ?? new List<RoomViewDTO>();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"HTTP Request Error: {ex.Message}. BaseAddress: {_httpClient.BaseAddress}");
                return new List<RoomViewDTO>();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching rooms: {ex.Message}");
                return new List<RoomViewDTO>();
            }
        }

        public async Task<RoomViewDTO?> GetRoomByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Fetching room {id} from: {_httpClient.BaseAddress}room/{id}");
                
                var response = await _httpClient.GetAsync($"room/{id}");

                _logger.LogInformation($"API Response Status: {response.StatusCode}");

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    _logger.LogWarning($"Room {id} not found");
                    throw new KeyNotFoundException($"Room with id {id} not found.");
                }

                response.EnsureSuccessStatusCode();

                var room = await response.Content.ReadFromJsonAsync<RoomViewDTO>();
                _logger.LogInformation($"Successfully fetched room {id}");
                return room;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"HTTP Request Error fetching room {id}: {ex.Message}");
                throw;
            }
        }

        public async Task<RoomDetailsDTO?> GetRoomDetailsAsync(int roomId)
        {
            try
            {
                _logger.LogInformation($"Fetching room details for {roomId} from: {_httpClient.BaseAddress}room/{roomId}");
                
                var resp = await _httpClient.GetAsync($"room/{roomId}");
                
                _logger.LogInformation($"API Response Status: {resp.StatusCode}");

                if (!resp.IsSuccessStatusCode)
                {
                    _logger.LogWarning($"Failed to fetch room details: {resp.StatusCode}");
                    return null;
                }

                var result = await resp.Content.ReadFromJsonAsync<RoomDetailsDTO>();
                _logger.LogInformation($"Successfully fetched room details for {roomId}");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching room details: {ex.Message}");
                return null;
            }
        }
    }
}
