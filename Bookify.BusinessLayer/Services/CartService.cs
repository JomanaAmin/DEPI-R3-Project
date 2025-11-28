using Bookify.BusinessLayer.Contracts;
using Bookify.BusinessLayer.DTOs.CartDTOs;
using Bookify.DAL.Entities;
using Bookify.DAL.Repositories;
using Bookify.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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
        private IGenericRepository<BookingItem> bookingItemsRepository;
        private IGenericRepository<CustomerProfile> customerRepository;
        private IRoomRepository roomRepository;

        public CartService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            cartItemRepo = _unitOfWork.CartItems;
            cartRepo = _unitOfWork.Carts;
            customerRepository = _unitOfWork.CustomerProfiles;
            roomRepository = _unitOfWork.Rooms;
            bookingItemsRepository = _unitOfWork.BookingItems;
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
            int cartId= await customerRepository.GetAllAsQueryable().AsNoTracking().Where(c=>c.CustomerId==customerId).Select(c=>c.Cart.CartId).SingleOrDefaultAsync();
            Console.WriteLine($"Cart ID: {cartId}");

            decimal pricePerNight = roomRepository.GetAllAsQueryable().AsNoTracking().Where(r => r.RoomId == cartItemDTO.RoomId).Select(r => r.RoomType.PricePerNight).FirstOrDefault();
            if (pricePerNight == 0)
            {
                // This handles both RoomId=0 and a valid ID for a room that doesn't exist.
                throw new Exception($"Room with ID {cartItemDTO.RoomId} not found or price is invalid.");
            }
            Console.WriteLine($"pricePerNight: {pricePerNight}");

            //This is more efficient as it only fetches the price per night instead of entire room object.
            if (cartItemDTO.CheckInDate > cartItemDTO.CheckOutDate) 
            {
                throw new Exception("Check-out date must be after check-in date");
            }
            int nights = (cartItemDTO.CheckOutDate - cartItemDTO.CheckInDate).Days;
            decimal subtotal = pricePerNight * nights;
            CartItem cartItem = new CartItem {
                RoomId=cartItemDTO.RoomId,
                CartId= cartId,
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
            CartViewDTO? cartViewDTO = await cartRepo.GetAllAsQueryable().AsNoTracking().Where(
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
                        * ci.Room.RoomType.PricePerNight)
                    }
                
                ).FirstOrDefaultAsync();
            if (cartViewDTO == null)
            {
                throw new Exception("Cart not found");
            }
            if (!cartViewDTO.Items.Any()) 
            {
                cartViewDTO.IsValid = true;
                cartViewDTO.ValidationMessage = "Cart is empty";
            }
            cartViewDTO.IsValid = true;
            foreach (var cartItem in cartViewDTO.Items)
            {

                cartItem.IsValid = await IsRoomAvailableAsync(cartItem.RoomId, cartItem.CheckInDate, cartItem.CheckOutDate);
                if (!cartItem.IsValid)
                {
                    cartViewDTO.IsValid = false;
                }
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
        public async Task<bool> ValidateCartItemsAsync(string customerId)
        {
            CustomerProfile? customer = await customerRepository.GetByIdAsync(customerId);
            if (customer == null)
            {
                throw new Exception("Customer not found");
            }
            int? cartId = await cartRepo.GetAllAsQueryable().AsNoTracking().Where(c => c.CustomerId == customerId).Select(c => c.CartId).FirstOrDefaultAsync();
            
            if (cartId == null)
            {
                throw new Exception("Cart not found");
            }
            
            var c = await cartItemRepo.GetAllAsQueryable().AsNoTracking().Where(ci => ci.CartId == cartId).ToListAsync();
            bool isValid = true;
            foreach (var cartItem in c) 
            {
                isValid = await IsRoomAvailableAsync(cartItem.RoomId, cartItem.CheckInDate, cartItem.CheckOutDate);
                if (!isValid) 
                {
                    return false;
                }
            }
            return isValid; //at the moment of checkout if a room is no longer available, this method returns false
        }
        //////////CHECKOUT SUMMARY//////////

        public async Task<CheckoutSummaryDTO> CalculateCheckoutSummaryAsync(string customerId)
        {
            CustomerProfile? customer = await customerRepository.GetByIdAsync(customerId);
            if (customer == null)
            {
                throw new Exception("Customer not found");
            }
            bool valid = await ValidateCartItemsAsync(customerId);
            if (!valid) 
            {
                return new CheckoutSummaryDTO 
                {
                    IsValid=false,
                    Total= 0,
                    TotalItemsCount= 0
                };

            }
            CheckoutSummaryDTO? checkoutSummary = await cartRepo.GetAllAsQueryable().AsNoTracking().Where(c => c.CustomerId == customerId).Select(c => new CheckoutSummaryDTO
            {
                TotalItemsCount = c.CartItems.Count,
                Total = c.CartItems.Sum(ci => EF.Functions.DateDiffDay(ci.CheckInDate, ci.CheckOutDate) * ci.Room.RoomType.PricePerNight),
                IsValid=valid
            }).FirstOrDefaultAsync();
            if (checkoutSummary == null) 
            {
                throw new Exception("Cart not found");
            }
            return checkoutSummary;
        }

        //////////CLEAR CART//////////

        public async Task ClearCartAsync(string customerId)
        {
            CustomerProfile? customer = await customerRepository.GetByIdAsync(customerId);
            if (customer == null)
            {
                throw new Exception("Customer not found");
            }
            Cart? cart = await cartRepo.GetAllAsQueryable().AsNoTracking().Where(c => c.CustomerId == customerId).Include(c=>c.CartItems).SingleOrDefaultAsync();
            if (cart == null) 
            {
                throw new Exception($"Cart for customer {customerId} not found");
            }
            //List<int> cartItemIds = await cartItemRepo.GetAllAsQueryable().AsNoTracking().Where(ci => ci.CartId == cartId).Select(ci => ci.CartItemId).ToListAsync();
            //foreach (int cartItemId in cartItemIds) 
            //{
            //    await cartItemRepo.Delete(cartItemId);
            //}
            if (!cart.CartItems.Any())
                return;

            cartItemRepo.DeleteRange(cart.CartItems);
            await _unitOfWork.SaveChangesAsync();

        }
       
        public void validateCustomerId(string customerId,int cartItemId) 
        {
            string? fetchedCustomerId = cartItemRepo.GetAllAsQueryable().AsNoTracking().Where(ci => ci.CartItemId == cartItemId).Select(ci => ci.Cart.CustomerId).FirstOrDefault();
            if (fetchedCustomerId != customerId)
            {
                throw new Exception("Unauthorized to perform this operation.");
            }
            if (fetchedCustomerId==null)
            {
                throw new Exception("Cart item not found or does not belong to the customer");
            }

        }
        public async Task<bool> IsRoomAvailableAsync(int roomId, DateTime checkIn, DateTime checkOut)
        {
            var overlappingBookings = await bookingItemsRepository.GetAllAsQueryable().AsNoTracking()
            .Where(bi => bi.RoomId == roomId)
            .Where(bi => bi.Booking.Status == BookingStatus.Confirmed)
            .Where(bi => bi.CheckOutDate > checkIn && checkOut > bi.CheckInDate).AnyAsync(); //this checks if overlap exists, will return true if exists
            return !overlappingBookings; //means overlap=false, room is available or vice versa
        }
        //public async Task CustomerExists(string customerId) 
        //{
        //    CustomerProfile? customer = await customerRepository.GetByIdAsync(customerId);
        //        if (customer == null)
        //        {
        //            throw new Exception("Customer not found");
        //}
        //}

    }
}
