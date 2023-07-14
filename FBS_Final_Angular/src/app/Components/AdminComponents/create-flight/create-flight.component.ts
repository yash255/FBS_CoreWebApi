import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormArray } from '@angular/forms';
import { FlightService } from 'src/app/Services/flight.service';
import { Flight, CabinDto,Cabin,FlightDto } from 'src/app/Models/FlightModel';
import { Router } from '@angular/router';

@Component({
  selector: 'app-create-flight',
  templateUrl: './create-flight.component.html',
  styleUrls: ['./create-flight.component.css']
})
export class CreateFlightComponent {
  flightDto: FlightDto = {
    flightNumber: '',
    departureAirport: '',
    arrivalAirport: '',
    departureTime: new Date(),
    arrivalTime: new Date(),
    price: 0,
    cabins: []
  };

  constructor(private flightService: FlightService,private router:Router) { }

  createFlight(): void {
    this.flightService.createFlight(this.flightDto)
      .subscribe(flight => console.log(flight));

      alert("Flight Created Successfully");
      this.router.navigate(['/admin']);


  }

  addCabin(): void {
    this.flightDto.cabins.push({ name: '', noOfSeats: 0 });
  }

  removeCabin(index: number): void {
    this.flightDto.cabins.splice(index, 1);
  }
}






