import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import jwt_decode from "jwt-decode";
import { JwtHelperService } from '@auth0/angular-jwt';
import { CancelBookingResult } from '../Models/CancelBookingResult';
import { tap } from 'rxjs/operators';


@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private baseUrl: string = "https://localhost:7176/api/"

  private userPayload: any;

  constructor(private http: HttpClient, private router: Router) {
    this.userPayload = this.decodedToken();
  }

  signup(userObj: any): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}Authenticate/register`, userObj);
  }

  signupadmin(userObj: any): Observable<any> {
    const headers = this.getAuthorizationHeaders();
    return this.http.post<any>(`${this.baseUrl}Authenticate/register-admin`, userObj, { headers });
  }

  login(loginObj: any): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}Authenticate/login`, loginObj)
      .pipe(
        tap((response) => {
          const token = response.token; // Assuming the API response contains a 'token' field
          this.setToken(token);
  
          // Check if user is admin
          const decodedToken: any = jwt_decode(token);
          const isAdmin = decodedToken.role === 'admin';
  
          if (!isAdmin) {
            // Not an admin, clear token and navigate to login page
            this.signOut();
          }
        })
      );
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
