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

        [HttpGet("bookings")]
        public IActionResult GetUserWithBookings()
        {
            try
            {
                var userWithBookingsDto = _userRepo.GetUserWithBookings();
                return Ok(userWithBookingsDto);
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new { Message = ex.Message });
            }
        }
    }
}
