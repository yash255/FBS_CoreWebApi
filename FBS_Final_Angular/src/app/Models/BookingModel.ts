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
export interface BookingResult {
  isSuccessful: boolean;
  errorMessage: string;
}
