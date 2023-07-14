import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BookingService } from 'src/app/Services/booking.service';
import { Router } from '@angular/router';


@Component({
  selector: 'app-booking',
  templateUrl: './booking.component.html',
  styleUrls: ['./booking.component.css']
})
export class BookingComponent implements OnInit {
  flightId!: number; // Add the "!" non-null assertion operator
  booking: any = {};
  errorMessage: string = '';

  constructor(private route: ActivatedRoute, private bookingService: BookingService, private router: Router) { }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      this.flightId = +params.get('flightId')!;
    });
  }

  bookFlight() {
    this.errorMessage = '';
  
    this.bookingService.bookFlight(this.flightId, this.booking)
      .subscribe(
        () => {
          // Booking successful
          alert('Booking successful');
          this.router.navigate(['/home']);
        },
        (error) => {
          if (error.message === 'Selected cabin class is not available') {
            alert('Selected cabin class is not available. Please choose another cabin class or try again later.');
          } else {
            this.errorMessage = error.message;
            alert("Something went wrong please try again.");
          }
        }
      );
  }
  
}
