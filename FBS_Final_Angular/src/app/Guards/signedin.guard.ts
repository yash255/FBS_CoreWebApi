import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router, CanActivateFn } from '@angular/router';
import { AuthService } from '../Services/auth.service';


@Injectable({
  providedIn: 'root'
})
export class AuthGuard2 implements CanActivate {
  constructor(private authService: AuthService, private router: Router) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    if (this.authService.isLoggedIn()) {
      this.router.navigate(['/home']);
      return false;
    } else {
      return true;
    }
  }

}

export const signedinGuard: CanActivateFn = (route, state) => {
  return true;
};
