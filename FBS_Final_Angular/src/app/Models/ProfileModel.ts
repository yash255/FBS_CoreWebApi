export interface Profile {
  id: string;
  email: string;
  bookings: Booking[];
}

export interface Booking {
  id: number;
  passengerName: string;
  email: string;
  phoneNumber: string;
  age: number;
  gender: string;
  cabinClass: string;
  noOfTicket: number;
}
