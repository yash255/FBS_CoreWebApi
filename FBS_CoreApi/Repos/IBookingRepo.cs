using System.Net;
using System.Net.Mail;
using Microsoft.EntityFrameworkCore;
using FBS_CoreApi.DTOs;
using FBS_CoreApi.Models;
using FBS_CoreApi.Data;
using static FBS_CoreApi.Repositories.BookingRepository;
using System.Text;

namespace FBS_CoreApi.Repositories
{
    public interface IBookingRepo
    {
        Task<BookFlightResult> BookFlight(int flightId, BookingDTO booking, string userId);
        Task<CancelBookingResult> CancelBooking(int bookingId, string userId);
    }

    public class BookingRepository : IBookingRepo
    {
        private readonly ApplicationDbContext _context;

        public BookingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<BookFlightResult> BookFlight(int flightId, BookingDTO booking, string userId)
        {
            var flight = await _context.Flights
                .Include(f => f.CabinClasses)
                .FirstOrDefaultAsync(f => f.Id == flightId);

            if (flight == null)
            {
                return new BookFlightResult(false, "Invalid flight ID");
            }

            if (booking.NoOfTicket < 1)
            {
                return new BookFlightResult(false, "Number of tickets must be at least 1");
            }

            if (booking.NoOfTicket > 10)
            {
                return new BookFlightResult(false, "You can book up to 10 tickets at a time");
            }

            var selectedCabin = flight.CabinClasses.FirstOrDefault(c => c.Name == booking.CabinClass);
            if (selectedCabin == null || selectedCabin.NoOfSeats < booking.NoOfTicket)
            {
                return new BookFlightResult(false, "Selected cabin class is not available");
            }

            // Calculate total price based on the number of tickets and flight price
            decimal totalPrice = flight.Price * booking.NoOfTicket;

            if (booking.CabinClass == "Business")
            {
                totalPrice = flight.Price*booking.NoOfTicket*1.3m; // Increase the price by 10%
                flight.CabinClasses.First(c => c.Name == "Business").NoOfSeats -= booking.NoOfTicket; // Decrease the number of available seats for the Business cabin
            }
            else if (booking.CabinClass == "First")
            {
                totalPrice = flight.Price * booking.NoOfTicket * 1.4m; // Increase the price by 20%
                flight.CabinClasses.First(c => c.Name == "First").NoOfSeats -= booking.NoOfTicket; // Decrease the number of available seats for the First cabin
            }
            else if (booking.CabinClass == "Economy")
            {
                totalPrice = flight.Price * booking.NoOfTicket * 1.1m; // Increase the price by 30%
                flight.CabinClasses.First(c => c.Name == "Economy").NoOfSeats -= booking.NoOfTicket; // Decrease the number of available seats for the Economy cabin
            }
            else if (booking.CabinClass == "Premium Economy")
            {
                totalPrice = flight.Price * booking.NoOfTicket * 1.2m; // Increase the price by 30%
                flight.CabinClasses.First(c => c.Name == "Premium Economy").NoOfSeats -= booking.NoOfTicket; // Decrease the number of available seats for the Economy cabin
            }
            else
            {
                return new BookFlightResult(false, "Invalid cabin class selected");
            }

            // Create a new Booking entity
            var newBooking = new Booking
            {
                PassengerName = booking.PassengerName,
                Email = booking.Email,
                PhoneNumber = booking.PhoneNumber,
                Age = booking.Age,
                Gender = booking.Gender,
                CabinClass = booking.CabinClass,
                NoOfTicket = booking.NoOfTicket,
                TotalPrice = totalPrice,
                FlightId = flight.Id,
                UserId = userId
            };

            // Add the new booking to the database
            _context.Bookings.Add(newBooking);
            await _context.SaveChangesAsync();

            // Send confirmation email to user
            await SendConfirmationEmailAsync(flight, booking, totalPrice, "email");

            return new BookFlightResult(true, "");
        }

        public async Task<CancelBookingResult> CancelBooking(int bookingId, string userId)
        {
            var booking = await _context.Bookings
                .Include(b => b.Flight)
                .FirstOrDefaultAsync(b => b.Id == bookingId && b.UserId == userId);

            if (booking == null)
            {
                return new CancelBookingResult(false, "Invalid booking ID");
            }

            // Check if the departure time of the flight has passed or it is within 5 minutes before departure
            var currentTime = DateTime.UtcNow;
            var departureTime = booking.Flight.DepartureTime;

            if (departureTime <= currentTime || departureTime.AddMinutes(-5) <= currentTime)
            {
                return new CancelBookingResult(false, "Cancellation is not allowed for this booking.");
            }

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            // Send cancellation confirmation email to user
            await SendCancellationConfirmationEmailAsync(booking);

            return new CancelBookingResult(true, "");
        }


        private async Task SendConfirmationEmailAsync(Flight flight, BookingDTO booking, decimal totalPrice, string fromEmail)
        {
            var message = new MailMessage();
            message.To.Add(new MailAddress(booking.Email));
            message.Subject = "Flight Booking Confirmation";
            var bodyBuilder = new StringBuilder();
            bodyBuilder.Append("<html><body>");
            bodyBuilder.Append($"<h2>Dear {booking.PassengerName},</h2>");
            bodyBuilder.Append("<p>Thank you for booking your flight with us. Your booking details are as follows:</p>");
            bodyBuilder.Append("<table border='1' cellpadding='10'><tbody>");
            bodyBuilder.Append($"<tr><td><b>Flight:</b></td><td>{flight.FlightNumber}</td></tr>");
            bodyBuilder.Append($"<tr><td><b>Date:</b></td><td>{flight.DepartureTime}</td></tr>");
            bodyBuilder.Append($"<tr><td><b>Passenger Name:</b></td><td>{booking.PassengerName}</td></tr>");
            bodyBuilder.Append($"<tr><td><b>Number of Tickets:</b></td><td>{booking.NoOfTicket}</td></tr>");
            bodyBuilder.Append($"<tr><td><b>Total Price:</b></td><td>{totalPrice}</td></tr>");
            bodyBuilder.Append("</tbody></table>");

            bodyBuilder.Append("<p>Please do not hesitate to contact us if you have any questions or concerns.</p>");
            bodyBuilder.Append("<p>Sincerely,<br>The Flight Booking Team</p>");
            bodyBuilder.Append("</body></html>");

            message.IsBodyHtml = true;
            message.Body = bodyBuilder.ToString();
            message.From = new MailAddress(fromEmail);

            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = "email",
                    Password = "password"
                };
                smtp.UseDefaultCredentials = false;

                smtp.Credentials = credential;
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;

                await smtp.SendMailAsync(message);
            }
        }





        private async Task SendCancellationConfirmationEmailAsync(Booking booking)
        {
            var flight = booking.Flight;
            var message = new MailMessage();
            message.To.Add(new MailAddress(booking.Email));
            message.Subject = "Flight Booking Cancellation Confirmation";
            var bodyBuilder = new StringBuilder();
            bodyBuilder.Append("<html><body>");
            bodyBuilder.Append($"<h2>Dear {booking.PassengerName},</h2>");
            bodyBuilder.Append("<p>Your flight booking cancellation has been processed successfully. The details of the cancelled booking are as follows:</p>");
            bodyBuilder.Append("<table border='1' cellpadding='10'><tbody>");
            bodyBuilder.Append($"<tr><td><b>Flight:</b></td><td>{flight.FlightNumber}</td></tr>");

            bodyBuilder.Append("</tbody></table>");

            bodyBuilder.Append("<p>If you have any questions or require further assistance, please feel free to contact us.</p>");
            bodyBuilder.Append("<p>Thank you for choosing our services.</p>");
            bodyBuilder.Append("<p>Sincerely,<br>The Flight Booking Team</p>");
            bodyBuilder.Append("</body></html>");

            message.IsBodyHtml = true;
            message.Body = bodyBuilder.ToString();
            message.From = new MailAddress("email");

            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = "email",
                    Password = "password"
                };
                smtp.UseDefaultCredentials = false;

                smtp.Credentials = credential;
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;

                await smtp.SendMailAsync(message);
            }
        }



        public class BookFlightResult
        {
            public bool IsSuccessful { get; }
            public string ErrorMessage { get; }

            public BookFlightResult(bool isSuccessful, string errorMessage)
            {
                IsSuccessful = isSuccessful;
                ErrorMessage = errorMessage;
            }
        }

        public class CancelBookingResult
        {
            public bool IsSuccessful { get; }
            public string ErrorMessage { get; }

            public CancelBookingResult(bool isSuccessful, string errorMessage)
            {
                IsSuccessful = isSuccessful;
                ErrorMessage = errorMessage;
            }
        }
    }
}
