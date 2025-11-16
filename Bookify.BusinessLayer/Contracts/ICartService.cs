using Bookify.BusinessLayer.DTOs.CartDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.BusinessLayer.Contracts
{
    public interface ICartService
    {

        Task AddItemToCartAsync(string userId, CartAddItemDTO itemDto);
        Task<CartViewDTO> RemoveItemFromCartAsync(string userId, int cartItemId);
        Task<CartViewDTO> UpdateItemDatesAsync(string userId, int cartItemId, DateTime checkInDate, DateTime checkOutDate);

        // Retrieval and Finalization
        Task<CartViewDTO> GetCartByUserIdAsync(string userId);
        Task ClearCartAsync(string userId);

        // Core Business Logic
        Task<bool> ValidateCartItemsAsync(string userId);
        Task<CheckoutSummaryDTO> CalculateCheckoutSummaryAsync(string userId);
        
    }
}
