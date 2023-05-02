using FBS_CoreApi.Data;
using FBS_CoreApi.DTOs;
using FBS_CoreApi.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Claims;

namespace FBS_CoreApi.Repositories
{
    public interface IUserRepo
    {
        UserWithBookingsDto GetUserWithBookings();
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
                    Email = user.Email,
                    Bookings = _context.Bookings
                        .Where(b => b.UserId == authenticatedUserId)
                        .Select(b => new BookingDTO
                        {
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

    }
}
