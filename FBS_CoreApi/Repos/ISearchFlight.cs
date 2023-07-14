using FBS_CoreApi.Data;
using FBS_CoreApi.DTOs;
using Microsoft.EntityFrameworkCore;

namespace FBS_CoreApi.Repos
{
    public interface ISearchRepo
    {
        Task<List<FlightDto>> GetFlightsAsync(string departureAirport, string arrivalAirport, DateTime departureTime);
      //  Task<FlightDto> GetFlightByIdAsync(int flightId);

    }

    public class SearchRepo : ISearchRepo
    {
        private readonly ApplicationDbContext _dbContext;

        public SearchRepo(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<FlightDto>> GetFlightsAsync(string departureAirport, string arrivalAirport, DateTime departureTime)
        {
            var flights = await _dbContext.Flights
                .Include(f => f.CabinClasses)
                .Where(f => f.DepartureAirport == departureAirport
                            && f.ArrivalAirport == arrivalAirport
                            && f.DepartureTime.Date == departureTime.Date)
                .ToListAsync();

            return flights.Select(flight => new FlightDto
            {
                Id = flight.Id,
                FlightNumber = flight.FlightNumber,
                DepartureAirport = flight.DepartureAirport,
                ArrivalAirport = flight.ArrivalAirport,
                DepartureTime = flight.DepartureTime,
                ArrivalTime = flight.ArrivalTime,
                Price = flight.Price,
                Cabins = flight.CabinClasses.Select(cabin => new CabinClassDto
                {
                    Name = cabin.Name,
                    NoOfSeats = cabin.NoOfSeats
                }).ToList()

            }).ToList();
        }

      
    

}



}
