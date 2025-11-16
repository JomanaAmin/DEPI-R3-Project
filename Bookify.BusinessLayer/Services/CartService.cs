using Bookify.BusinessLayer.Contracts;
using Bookify.BusinessLayer.DTOs.CartDTOs;
using Bookify.DAL.Entities;
using Bookify.DAL.Repositories;
using Bookify.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.BusinessLayer.Services
{
    internal class CartService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IGenericRepository<CartItem> cartItemRepo;
        private IGenericRepository<Cart> cartRepo;
        private IGenericRepository<CustomerProfile> customerRepository;
        public CartService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            cartItemRepo = _unitOfWork.CartItems;
            cartRepo = _unitOfWork.Carts;
            customerRepository = _unitOfWork.CustomerProfiles;
        }
        /////////ADD ITEM TO CART//////////

        public async Task AddItemToCartAsync(string customerId, CartAddItemDTO cartItemDTO)
        {
            CustomerProfile? customer = await customerRepository.GetByIdAsync(customerId);
            if (customer == null)
            {
                throw new Exception("Customer not found");
            } //this customer has no account! ask them to create one.

            CartItem cartItem = new CartItem {
                RoomId=cartItemDTO.RoomId,
                CartId=cartItemDTO.CartId,
                CheckInDate=cartItemDTO.CheckInDate,
                CheckOutDate=cartItemDTO.CheckOutDate,
                PreviewImageUrl=
                //,Nights =(cartItemDTO.CheckOutDate - cartItemDTO.CheckInDate).Days
            };

            await cartItemRepo.CreateAsync(cartItem);
            await _unitOfWork.SaveChangesAsync();
            return;
        }
        /////////GET CART//////////
      
        public async Task<CartViewDTO> GetCartByUserIdAsync(string customerId)
        {
            CustomerProfile? customer = await customerRepository.GetByIdAsync(customerId);
            if (customer == null)
            {
                throw new Exception("Customer not found");
            }
            CartViewDTO? cartViewDTO = cartRepo.GetAllAsQueryable().AsNoTracking().Where(
                c => c.CustomerId == customerId
                ).Select(
                    c=> new CartViewDTO {
                        CartId = c.CartId,
                        Items = c.CartItems.Select(ci => new CartItemDTO
                        {
                            CartItemId=ci.CartItemId,
                            CartId=ci.CartId,
                            RoomId=ci.RoomId,
                            CheckInDate=ci.CheckInDate,
                            CheckOutDate=ci.CheckOutDate,
                            PricePerNight=ci.Room.RoomType.PricePerNight,
                            Subtotal=ci.Subtotal,
                            Nights=ci.Nights,
                            PreviewImageUrl=ci.Room.RoomImages.Select(ri=>ri.ImageUrl).FirstOrDefault()

                        }).ToList(),
                        Total = c.CartItems.Sum(ci => ci.Subtotal),
                    }
                
                ).FirstOrDefault();
            if (cartViewDTO == null)
            {
                throw new Exception("Cart not found");
            }
            if (!cartViewDTO.Items.Any()) 
            {
                cartViewDTO.IsValid = true;
                cartViewDTO.ValidationMessage = "Cart is empty";
            }
            return cartViewDTO;
        }
        public async Task<CartViewDTO> RemoveItemFromCartAsync(string customerId, int cartItemId)
        {
            CartItem? cartItem = await cartItemRepo.Delete(cartItemId);
            if (cartItem == null)
            {
                throw new Exception("Cart item not found");
            }
            await _unitOfWork.SaveChangesAsync();
            return await GetCartByUserIdAsync(customerId);
        }

        public Task<CartViewDTO> UpdateItemDatesAsync(string customerId, int cartItemId, DateTime checkInDate, DateTime checkOutDate)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ValidateCartItemsAsync(string customerId)
        {
            throw new NotImplementedException();
        }
        public Task<CheckoutSummaryDTO> CalculateCheckoutSummaryAsync(string customerId)
        {
            throw new NotImplementedException();
        }

        public Task ClearCartAsync(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
