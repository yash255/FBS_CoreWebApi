import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FlightDto,CabinDto } from 'src/app/Models/FlightModel';
import { FlightService } from 'src/app/Services/flight.service';
import { Router } from '@angular/router';


@Component({
  selector: 'app-update-flight',
  templateUrl: './update-flight.component.html',
  styleUrls: ['./update-flight.component.css']
})
export class UpdateFlightComponent implements OnInit {
  flightId!: number;
  flight!: FlightDto;

  constructor(
    private route: ActivatedRoute,
    private flightService: FlightService,
    private router:Router
  ) { }

  ngOnInit(): void { 
    this.route.params.subscribe(params => {
      this.flightId = +params['id'];
      this.loadFlight();
    });
  }

  loadFlight(): void {
    this.flightService.getFlightById(this.flightId).subscribe(
      (flight: FlightDto) => {
        this.flight = flight;
      },
      (error: any) => {
        console.log('An error occurred while loading the flight:', error);
      }
    );
  }

  updateFlight(): void {
    this.flightService.updateFlight(this.flightId, this.flight).subscribe(
      () => {
        alert('Flight updated successfully');
        this.router.navigate(['/admin']);
      },
      (error: any) => {
        console.log('An error occurred while updating the flight:', error);
        // Handle the error appropriately (e.g., show an error message)
      }
    );
  }

  

}
