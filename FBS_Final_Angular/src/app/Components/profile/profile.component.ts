import { Component, OnInit } from '@angular/core';
import { ProfileService } from 'src/app/Services/profile.service';
import { Profile, Booking } from 'src/app/Models/ProfileModel';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {
  bookings: Booking[] = [];
  errorMessage: string = '';

  constructor(private profileService: ProfileService) { }

  ngOnInit() {
    this.getUserBookings();
  }

  getUserBookings() {
    this.profileService.getUserBookings().subscribe(
      (profile: Profile) => {
        this.bookings = profile.bookings;
      },
      (error) => {
        this.errorMessage = error.message;
      }
    );
  }

  cancelBooking(bookingId: number) {
    if (confirm('Are you sure you want to cancel this booking?')) {
      this.profileService.cancelBooking(bookingId).subscribe(
        () => {
          this.getUserBookings();
          alert('Booking cancelled successfully');
        },
        (error) => {
          this.errorMessage = error.message;
          alert(this.errorMessage);
        }
      );
    }
  }
}
