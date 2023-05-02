using FBS_CoreApi.DTOs;
using System;
using System.Collections.Generic;

namespace FBS_CoreApi.DTOs
{
    public class FlightDto
    {
        public string FlightNumber { get; set; }

        public string DepartureAirport { get; set; }
        public string ArrivalAirport { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public decimal Price { get; set; }

        public List<CabinClassDto> Cabins { get; set; }
    }
}
