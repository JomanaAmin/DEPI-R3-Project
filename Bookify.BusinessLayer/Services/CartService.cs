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
        private IRoomRepository roomRepository;
        public CartService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            cartItemRepo = _unitOfWork.CartItems;
            cartRepo = _unitOfWork.Carts;
            customerRepository = _unitOfWork.CustomerProfiles;
            roomRepository = _unitOfWork.Rooms;
        }

        //////////ADD ITEM TO CART//////////
        public async Task AddItemToCartAsync(string customerId, CartAddItemDTO cartItemDTO)
        {
            CustomerProfile? customer = await customerRepository.GetByIdAsync(customerId);
            if (customer == null)
            {
                throw new Exception("Customer not found");
            } //this customer has no account! ask them to create one.

            //Room? room = await roomRepository.GetByIdAsync(cartItemDTO.RoomId);
            //if (room == null)
            //{
            //    throw new Exception("Room not found");
            //} //the room they are trying to book does not exist.
            //decimal subtotal = (room?.RoomType?.PricePerNight ?? 0) * nights;
            //this method fetches entire room object including room type to get price per night.

            decimal pricePerNight = roomRepository.GetAllAsQueryable().AsNoTracking().Where(r => r.RoomId == cartItemDTO.RoomId).Select(r => r.RoomType.PricePerNight).FirstOrDefault();
            //This is more efficient as it only fetches the price per night instead of entire room object.
            int nights = (cartItemDTO.CheckOutDate - cartItemDTO.CheckInDate).Days;
            decimal subtotal = pricePerNight * nights;
            CartItem cartItem = new CartItem {
                RoomId=cartItemDTO.RoomId,
                CartId=cartItemDTO.CartId,
                CheckInDate=cartItemDTO.CheckInDate,
                CheckOutDate=cartItemDTO.CheckOutDate,
                Nights =nights,
                Subtotal=subtotal
            };
            await cartItemRepo.CreateAsync(cartItem);
            await _unitOfWork.SaveChangesAsync();
            return;
        }

        //////////GET CART//////////
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
                            Nights= EF.Functions.DateDiffDay(ci.CheckInDate, ci.CheckOutDate),
                            Subtotal = EF.Functions.DateDiffDay(ci.CheckInDate, ci.CheckOutDate) * ci.Room.RoomType.PricePerNight,
                            PreviewImageUrl=ci.Room.RoomImages.Select(ri=>ri.ImageUrl).FirstOrDefault()??string.Empty

                        }).ToList(),
                        Total = c.CartItems.Sum(ci =>
                        EF.Functions.DateDiffDay(ci.CheckInDate, ci.CheckOutDate)
                        * ci.Room.RoomType.PricePerNight),
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
        //////////REMOVE ITEM FROM CART//////////
        public async Task<CartViewDTO> RemoveItemFromCartAsync(string customerId, int cartItemId)
        {
            validateCustomerId(customerId, cartItemId);
            CartItem? cartItem = await cartItemRepo.Delete(cartItemId);
            //if (cartItem == null)
            //{
            //    throw new Exception("Cart item not found");
            //} already checked above
            await _unitOfWork.SaveChangesAsync();
            return await GetCartByUserIdAsync(customerId);
        }
        //////////UPDATE ITEM IN CART//////////
        public async Task<CartViewDTO> UpdateItemDatesAsync(string customerId, CartItemUpdateDatesDTO cartDTO)
        {
            validateCustomerId(customerId, cartDTO.CartItemId);
            //CartItem? cartItem = cartItemRepo.GetAllAsQueryable().AsNoTracking().Where(ci => ci.CartItemId == cartDTO.CartItemId).FirstOrDefault();
            CartItem? cartItem = await cartItemRepo.GetByIdAsync(cartDTO.CartItemId);
            if (cartItem == null)
            {
                throw new Exception("Cart item not found");
            }
            int nights = (cartDTO.NewCheckOutDate - cartDTO.NewCheckInDate).Days;
            cartItem.CheckInDate = cartDTO.NewCheckInDate;
            cartItem.CheckOutDate = cartDTO.NewCheckOutDate;
            cartItem.Nights = nights;
            decimal pricePerNight = roomRepository.GetAllAsQueryable().AsNoTracking().Where(r => r.RoomId == cartItem.RoomId).Select(r => r.RoomType.PricePerNight).FirstOrDefault();

            cartItem.Subtotal = pricePerNight * nights;
            cartItemRepo.Update(cartItem);
            await _unitOfWork.SaveChangesAsync();
            return await GetCartByUserIdAsync(customerId);
        }
        //////////VALIDATE CART ITEMS//////////
        public Task<bool> ValidateCartItemsAsync(string customerId)
        {
            throw new NotImplementedException();
        }
        //////////CHECKOUT SUMMARY//////////

        public Task<CheckoutSummaryDTO> CalculateCheckoutSummaryAsync(string customerId)
        {
            throw new NotImplementedException();
        }

        public Task ClearCartAsync(string userId)
        {
            throw new NotImplementedException();
        }
        public void validateCustomerId(string customerId,int cartItemId) 
        {
            string? fetchedCustomerId = cartItemRepo.GetAllAsQueryable().AsNoTracking().Where(ci => ci.CartItemId == cartItemId).Select(ci => ci.Cart.CustomerId).FirstOrDefault() ?? string.Empty;

            if (fetchedCustomerId==null)
            {
                throw new Exception("Cart item not found or does not belong to the customer");
            }
            if (fetchedCustomerId != customerId)
            {
                throw new Exception("Unauthorized to perform this operation.");
            }
        }
    }
}
