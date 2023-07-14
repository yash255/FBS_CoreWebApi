import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import {  Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Observable } from 'rxjs/internal/Observable';

@Injectable({
  providedIn: 'root'
})
export class BookingService {

  private bookUrl: string = "https://localhost:7176/api/Booking/"
  private userPayload: any;


  constructor(private http:HttpClient,private router:Router) { }

  bookFlight(flightId: number, booking: any): Observable<any> {
    const headers = this.getAuthorizationHeaders();

    return this.http.post(`${this.bookUrl}${flightId}`, booking,{headers});
  }


















  
setToken(token: string) {
  localStorage.setItem("token", token);
}

getToken() {
  return localStorage.getItem("token");
}

getAuthorizationHeaders(): HttpHeaders {
  return new HttpHeaders({
    'Content-Type': 'application/json',
    Authorization: `Bearer ${this.getToken()}`
  });
}

isLoggedIn(): boolean {
  return !!localStorage.getItem("token"); // 2 exclamation marks to convert string to boolean
}

decodedToken() {
  const jwtHelper = new JwtHelperService();
  const token = this.getToken()!;
  console.log(jwtHelper.decodeToken(token));
  return jwtHelper.decodeToken(token);
}

getRoleFromToken() {
  if (this.userPayload)
    return this.userPayload.role;
}

isAdminLoggedIn(): boolean {
  return !!localStorage.getItem("token"); 
}

signOut() {
  localStorage.clear();
 
  this.router.navigate(['login']);
}
}