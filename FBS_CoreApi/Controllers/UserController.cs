using FBS_CoreApi.DTOs;
using FBS_CoreApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FBS_CoreApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepo _userRepo;

        public UserController(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }

        [HttpGet("{userId}/bookings")]
        public IActionResult GetUserWithBookings(string userId)
        {
            try
            {
                var userWithBookingsDto = _userRepo.GetUserWithBookings(userId);
                return Ok(userWithBookingsDto);
            }
            catch (UnauthorizedAccessException ex)
            {
                if (ex.Message == "Please login and register.")
                {
                    return Unauthorized(new { Message = ex.Message });
                }
                else
                {
                    return StatusCode(403, new { Message = ex.Message });
                }
            }
        }
    }
}