using Bookify.BusinessLayer.Contracts;
using Bookify.BusinessLayer.DTOs.CartDTOs;
using Bookify.DAL.Entities;
using Bookify.DAL.Repositories;
using Bookify.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.BusinessLayer.Services
{
    internal class BookingService : IBookingService
    {
        private readonly IUnitOfWork unitOfWork;
        private IGenericRepository<CustomerProfile> customerProfileRepository;
        private IGenericRepository<Booking> bookingRepository;
        private IGenericRepository<BookingItem> bookingItemsRepository;
        private readonly ICartService cartService;
        public BookingService(IUnitOfWork unitOfWork, ICartService cartService)
        {
            this.unitOfWork = unitOfWork;
            bookingRepository = unitOfWork.Bookings;
            bookingItemsRepository = unitOfWork.BookingItems;
            this.cartService = cartService;
            customerProfileRepository = unitOfWork.CustomerProfiles;
        }

        public async Task CreateBookingFromCartAsync(string customerId) 
        {
            CartViewDTO cart = cartService.GetCartByUserIdAsync(customerId).Result;
            if (!cart.IsValid)
            {

                Error error = new Error("Validation Error", $"Cannot create booking from cart: {cart.ValidationMessage}", ErrorType.Validation);
                throw new CustomException(error);
            }
            Booking newBooking =  new Booking
            {
                CustomerId = customerId,
                BookingDate = DateTime.UtcNow,
                Status = BookingStatus.Confirmed,
                TotalAmount = cart.Total
            };
            await bookingRepository.CreateAsync(newBooking);
            await unitOfWork.SaveChangesAsync(); //to get bookingId
            List<BookingItem> bookingItems = cart.Items.Select(item => new BookingItem
            {
                BookingId = newBooking.BookingId,
                RoomId = item.RoomId,
                CheckInDate = item.CheckInDate,
                CheckOutDate = item.CheckOutDate,
                Subtotal = item.Subtotal,
                Nights = item.Nights
            }).ToList();
            foreach (var bookingItem in bookingItems)
            {
                await bookingItemsRepository.CreateAsync(bookingItem);

            }
            await unitOfWork.SaveChangesAsync();
            await cartService.ClearCartAsync(customerId);
        }

    }
}
