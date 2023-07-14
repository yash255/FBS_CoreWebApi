using FBS_CoreApi.Data;
using FBS_CoreApi.Models;

namespace FBS_CoreApi.DTOs
{
    public class UserWithBookingsDto
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public List<BookingDTO> Bookings { get; set; }
    }
}
