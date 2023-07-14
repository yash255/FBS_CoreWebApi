import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router, CanActivateFn } from '@angular/router';
import { AuthService } from '../Services/auth.service';


@Injectable({
  providedIn: 'root'
})
export class AuthGuard1 implements CanActivate {
  constructor(private authService: AuthService, private router: Router) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    if (this.authService.isLoggedIn()) {
      return true;
    } else {
      this.router.navigate(['/login']);
      return false;
    }
  }
}

export const authGuard: CanActivateFn = (route, state) => {
  return true;
};


// import { CanActivateFn, Router } from '@angular/router';
// import { AuthService } from '../Services/auth.service';


// export const authGuard: CanActivateFn = (route, state) => {
//   if (AuthService.isLoggedIn()) {
//     return true;
//   } else {
//     Router.navigate(['/login']);
//     return false;
//   }
// };
