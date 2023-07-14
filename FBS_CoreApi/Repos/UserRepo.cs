using FBS_CoreApi.Data;
using FBS_CoreApi.DTOs;
using FBS_CoreApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;

namespace FBS_CoreApi.Repositories
{
    public interface IUserRepo
    {
        UserWithBookingsDto GetUserWithBookings();
        bool CancelBooking(int bookingId);
    }

    public class UserRepo : IUserRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserRepo(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public UserWithBookingsDto GetUserWithBookings()
        {
            try
            {
                var authenticatedUserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (authenticatedUserId == null)
                {
                    throw new UnauthorizedAccessException("Please login or register.");
                }

                var user = _context.Users.FirstOrDefault(u => u.Id == authenticatedUserId);

                if (user == null)
                {
                    return null;
                }

                var userWithBookingsDto = new UserWithBookingsDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    Bookings = _context.Bookings
                        .Where(b => b.UserId == authenticatedUserId)
                        .Select(b => new BookingDTO
                        {
                            Id = b.Id,
                            PassengerName = b.PassengerName,
                            Email = b.Email,
                            PhoneNumber = b.PhoneNumber,
                            Age = b.Age,
                            Gender = b.Gender,
                            CabinClass = b.CabinClass,
                            NoOfTicket = b.NoOfTicket
                        })
                        .ToList()
                };

                return userWithBookingsDto;
            }
            catch (Exception ex)
            {
                throw new UnauthorizedAccessException("An error occurred while processing your request. Please try again later.");
            }
        }

        public bool CancelBooking(int bookingId)
        {
            try
            {
                var authenticatedUserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (authenticatedUserId == null)
                {
                    throw new UnauthorizedAccessException("Please login or register.");
                }

                var booking = _context.Bookings
                    .Include(b => b.Flight)
                    .FirstOrDefault(b => b.Id == bookingId && b.UserId == authenticatedUserId);

                if (booking == null)
                {
                    return false;
                }

                // Check if the departure time of the flight has passed or it is within 5 minutes before departure
                var currentTime = DateTime.UtcNow;
                var departureTime = booking.Flight.DepartureTime;

                if (departureTime <= currentTime || departureTime.AddMinutes(-5) <= currentTime)
                {
                    throw new InvalidOperationException("Cancellation is not allowed for this booking.");
                }

                _context.Bookings.Remove(booking);
                _context.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while canceling the booking. Please try again later.");
            }
        }

    }
}
