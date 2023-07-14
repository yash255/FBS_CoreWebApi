import { Component, OnInit } from '@angular/core';
import { FlightService } from 'src/app/Services/flight.service';
import { Flight } from 'src/app/Models/FlightModel';
import { Router } from '@angular/router';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit {
  flights: Flight[] | undefined;
  errorMessage: string | undefined;

  constructor(private flightService: FlightService, private router: Router) { }

  ngOnInit() {
    this.getFlights();
  }

  createFlight() {
    this.router.navigate(['/create-flight']);
  }

  registeradmin() {
    this.router.navigate(['/register-admin']);
  }

  updateflight(id: number) {
    this.router.navigate(['/update-flight/'+id])

  }

  getFlights() {
    this.flightService.getFlights().subscribe(
      (flights:any) => {
        this.flights = flights;
        this.errorMessage = undefined; // Clear any previous error message
      },
      (error:any) => {
        this.errorMessage = 'Please login as an Admin to access.';
        console.log('An error occurred while retrieving flights:', error);
      }
    );
  }

  deleteFlight(id: number) {
    if (confirm("Are you sure you want to delete this flight?")) {
      this.flightService.deleteFlight(id).subscribe(
        () => {
          this.getFlights();

          this.errorMessage = undefined; // Clear any previous error message
        },
        (error: any) => {
          this.errorMessage = 'An error occurred while deleting the flight.';

          console.log('An error occurred while deleting the flight:', error);
        }
      );
    }
  }
}
