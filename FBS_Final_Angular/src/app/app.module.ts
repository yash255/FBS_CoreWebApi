import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './Components/login/login.component';
import { SignupComponent } from './Components/signup/signup.component';
import { NavbarComponent } from './Components/navbar/navbar.component';
import { HomeComponent } from './Components/home/home.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { FormsModule } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { AdminComponent } from './Components/AdminComponents/admin/admin.component';
import { CreateFlightComponent } from './Components/AdminComponents/create-flight/create-flight.component';
import { RegisterAdminComponent } from './Components/AdminComponents/register-admin/register-admin.component';
import { UpdateFlightComponent } from './Components/AdminComponents/update-flight/update-flight.component';
import { SearchComponent } from './Components/search/search.component';
import { BookingComponent } from './Components/booking/booking.component';
import { ProfileComponent } from './Components/profile/profile.component';



@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    SignupComponent,
    NavbarComponent,
    HomeComponent,
    
    AdminComponent,
    CreateFlightComponent,
    RegisterAdminComponent,
    UpdateFlightComponent,
    SearchComponent,
    BookingComponent,
    ProfileComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    MatSnackBarModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
