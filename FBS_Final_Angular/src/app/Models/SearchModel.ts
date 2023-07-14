// export class SearchModel {
//     departureAirport: string;
//     arrivalAirport: string;
//     departureTime: string;
  
//     constructor(departureAirport: string, arrivalAirport: string, departureTime: string) {
    
//       this.departureAirport = departureAirport;
//       this.arrivalAirport = arrivalAirport;
//       this.departureTime= departureTime;
//     }
//   }
  

export interface Flight {
  id: number;
  flightNumber: string;
  departureAirport: string;
  arrivalAirport: string;
  departureTime: Date;
  arrivalTime: Date;
  price: number;
  cabins: CabinClass[];
}

export interface CabinClass {
  name: string;
  noOfSeats: number;
}
