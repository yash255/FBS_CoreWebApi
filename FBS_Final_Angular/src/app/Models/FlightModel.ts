export interface FlightDto {
  flightNumber: string;
  departureAirport: string;
  arrivalAirport: string;
  departureTime: Date;
  arrivalTime: Date;
  price: number;
  cabins: CabinDto[];
}

export interface Flight {
  id: number;
  flightNumber: string;
  departureAirport: string;
  arrivalAirport: string;
  departureTime: Date;
  arrivalTime: Date;
  price: number;
  cabinClasses: Cabin[];
}

export interface CabinDto {
  name: string;
  noOfSeats: number;
}

export interface Cabin {
  name: string;
  numberOfSeats: number;
}

export interface FlightID {
  id: number;
}

