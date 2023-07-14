import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './Components/login/login.component';
import { SignupComponent } from './Components/signup/signup.component';
import { NavbarComponent } from './Components/navbar/navbar.component';
import { HomeComponent } from './Components/home/home.component';
import { AuthGuard1 } from './Guards/auth.guard';
import { AuthGuard2 } from './Guards/signedin.guard';
import { AdminComponent } from './Components/AdminComponents/admin/admin.component';
import { CreateFlightComponent } from './Components/AdminComponents/create-flight/create-flight.component';
import { RegisterAdminComponent } from './Components/AdminComponents/register-admin/register-admin.component';
import { UpdateFlightComponent } from './Components/AdminComponents/update-flight/update-flight.component';
import { SearchComponent } from './Components/search/search.component';
import { BookingComponent } from './Components/booking/booking.component';
import { ProfileComponent } from './Components/profile/profile.component';



const routes: Routes = [
  {path:'login',component:LoginComponent, canActivate:[AuthGuard2]},
  {path:'signup',component:SignupComponent, canActivate:[AuthGuard2]},
  {path:'navbar',component:NavbarComponent},
  {path:'',component:HomeComponent},
  {path:'home',component:HomeComponent},
  {path:'admin',component:AdminComponent},
  {path: 'create-flight', component: CreateFlightComponent},
  {path: 'register-admin', component: RegisterAdminComponent},
  {path:'update-flight/:id',component:UpdateFlightComponent},
  {path:'search',component:SearchComponent},
  {path:'booking/:flightId',component:BookingComponent,canActivate:[AuthGuard1]},
  {path:'profile',component:ProfileComponent}


];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
