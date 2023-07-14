import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';

@Injectable({
  providedIn: 'root'
})
export class SearchService {

  private searchUrl: string = "https://localhost:7176/Search/"


  constructor(private http:HttpClient) { }

  searchFlights(search: any): Observable<any> {
    return this.http.get(`${this.searchUrl}?departureAirport=${search.departureAirport}&arrivalAirport=${search.arrivalAirport}&departureTime=${search.departureTime}`);
  }
}
