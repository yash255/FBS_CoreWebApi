using FBS_CoreApi.Data;
using FBS_CoreApi.DTOs;
using FBS_CoreApi.Models;
using FlightBooking.DTOs;
using Microsoft.EntityFrameworkCore;




namespace FlightBooking.Repos
{
    public interface IAdminRepo
    {

        Task<IEnumerable<Flight>> GetFlights();

        Task<Flight> GetFlightByNumber(string number);
        Task<Flight> CreateFlight(FlightDto flightDto);
        Task UpdateFlight(int id, AdminFlightDto flightDto);

        Task DeleteFlight(int id);
    }



    public class AdminRepository : IAdminRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public AdminRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;

        }

        public async Task<IEnumerable<Flight>> GetFlights()
        {
            return await _context.Flights
                .Include(flight => flight.CabinClasses)
                .ToListAsync();
        }





        public async Task<Flight> GetFlightByNumber(string number)
        {
            return await _context.Flights
                .Include(f => f.CabinClasses)
                .FirstOrDefaultAsync(f => f.FlightNumber == number);
        }


        public async Task<Flight> CreateFlight(FlightDto flightDto)
        {
            var existingFlight = await _context.Flights.FirstOrDefaultAsync(f => f.FlightNumber == flightDto.FlightNumber);
            if (existingFlight != null)
            {
                throw new ArgumentException("A flight with this number already exists");
            }

            if (flightDto.DepartureTime < DateTime.Now)
            {
                throw new ArgumentException("Error! Departure time cannot be in the past.");
            }

            if (flightDto.ArrivalTime < flightDto.DepartureTime)
            {
                throw new ArgumentException("Error! Arrival time cannot be before departure time.");
            }

            if (flightDto.Price <= 0)
            {
                throw new ArgumentException("Error! Price cannot be less than or eqaul to zero.");
            }

            

            var cabins = new List<CabinClass>();
            foreach (var cabinDto in flightDto.Cabins)
            {
                var cabin = new CabinClass
                {
                    Name = cabinDto.Name,
                    NoOfSeats = cabinDto.NoOfSeats,


                };
                if (cabinDto.NoOfSeats <= 0)
                {
                    throw new ArgumentException("Error! Number of seats cannot be less than or equal to zero.");
                }
                if (!new string[] { "Business", "Economy", "First","Premium Economy" }.Contains(cabinDto.Name))
                {
                    throw new ArgumentException("Error! Cabin name should be 'Business', 'Economy', 'First' or 'Premium Economy'");
                }
                if (cabins.Any(c => c.Name == cabinDto.Name))
                {
                    throw new ArgumentException("Error! A cabin with the same name already exists.");
                }
                cabins.Add(cabin);
            }

            var flight = new Flight
            {
                FlightNumber = flightDto.FlightNumber,
                DepartureAirport = flightDto.DepartureAirport,
                ArrivalAirport = flightDto.ArrivalAirport,
                DepartureTime = flightDto.DepartureTime,
                ArrivalTime = flightDto.ArrivalTime,
                Price = flightDto.Price,
                CabinClasses = cabins
            };

            await _context.Flights.AddAsync(flight);
            await _context.SaveChangesAsync();

            return flight;
        }



        public async Task UpdateFlight(int id, AdminFlightDto flightDto)
        {
            var flight = await _context.Flights.Include(f => f.CabinClasses).FirstOrDefaultAsync(f => f.Id == id);

            if (flight == null)
            {
                throw new ArgumentException("Flight not found");
            }

            if (flightDto.DepartureTime < DateTime.Now)
            {
                throw new ArgumentException("Error! Departure time cannot be in the past.\n\n");
            }

            if (flightDto.ArrivalTime < flightDto.DepartureTime)
            {
                throw new ArgumentException("Error! Arrival time cannot be before departure time.\n\n");
            }

            if (flightDto.Price <= 0)
            {
                throw new ArgumentException("Error! Price cannot be less than or eqaul to zero.");
            }

            flight.FlightNumber = flightDto.FlightNumber;
            flight.DepartureAirport = flightDto.DepartureAirport;
            flight.ArrivalAirport = flightDto.ArrivalAirport;
            flight.DepartureTime = flightDto.DepartureTime;
            flight.ArrivalTime = flightDto.ArrivalTime;
            flight.Price = flightDto.Price;

            var updatedCabins = new List<CabinClass>();

            // update existing cabin classes or add new ones
           // var cabins = new List<CabinClass>();

            foreach (var cabinDto in flightDto.CabinClasses)
            {
                var existingCabin = flight.CabinClasses.FirstOrDefault(c => c.Name == cabinDto.Name);

                if (existingCabin != null)
                {
                    // update existing cabin
                    existingCabin.NoOfSeats = cabinDto.NoOfSeats;
                    updatedCabins.Add(existingCabin);
                }
                else
                {
                    // add new cabin
                    var newCabin = new CabinClass
                    {
                        Name = cabinDto.Name,
                        NoOfSeats = cabinDto.NoOfSeats
                    };
                    updatedCabins.Add(newCabin);
                }

                if (cabinDto.NoOfSeats <= 0)
                {
                    throw new ArgumentException("Error! Number of seats cannot be less than or equal to zero.");
                }
                if (updatedCabins.Any(c => c.Name == cabinDto.Name))
                {
                    throw new ArgumentException("Error! A cabin with the same name already exists.");
                }
            }

           

            // update flight's cabin classes
            flight.CabinClasses = updatedCabins;

            await _context.SaveChangesAsync();
        }





        public async Task DeleteFlight(int id)
        {
            var flight = await _context.Flights.FindAsync(id);

            if (flight == null)
            {
                throw new ArgumentException("Flight not found");
            }

            _context.Flights.Remove(flight);
            await _context.SaveChangesAsync();
        }
    }
}