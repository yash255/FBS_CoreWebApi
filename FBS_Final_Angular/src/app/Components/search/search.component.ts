import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Flight } from 'src/app/Models/SearchModel';
import { SearchService } from 'src/app/Services/search.service';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css']
})
export class SearchComponent implements OnInit{

  search: any = {};
  flights: any[] = [];
  errorMessage: string = '';

  constructor(private searchService: SearchService, private router: Router) { }

  ngOnInit() {
  }

  searchFlights() {
    this.flights = [];
    this.errorMessage = '';

    this.searchService.searchFlights(this.search)
      .subscribe(
        (flights:any) => {
          this.flights = flights;
        },
        (error:any) => {
          this.errorMessage = error.message;
        }
      );
  }

  bookFlight(flightId: number) {
    this.router.navigate(['/booking', flightId]);
  }

}
