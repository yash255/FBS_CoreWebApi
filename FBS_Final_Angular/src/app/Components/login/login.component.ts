import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/Services/auth.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { LoginModel } from 'src/app/Models/LoginModel';
import { UserStoreService } from 'src/app/Services/user-store.service';


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  loginForm!: FormGroup;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router,
    private snackBar: MatSnackBar,
    private userStore: UserStoreService

  ) {
    this.loginForm = this.fb.group({
      email: ['', Validators.compose([Validators.required, Validators.email])],
      password: ['', Validators.required]
    });
  }

  login() {
    // Check if the form is valid
    if (this.loginForm.invalid) {
      // Mark the form controls as touched to trigger validation messages
      this.loginForm.markAllAsTouched();
      return; // Stop login if form is invalid
    }

    // Extract email and password from the form
    const email = this.loginForm.get('email')?.value;
    const password = this.loginForm.get('password')?.value;

    // Create login object
    const loginObj = { email, password };

    // Call the login method from the AuthService with the login object
    this.authService.login(loginObj).subscribe(
      (response) => {
        this.loginForm.reset();
        this.authService.setToken(response.token);
        const tokenPayload = this.authService.decodedToken();
        // this.userStore.setFullNameForStore(tokenPayload.name);
        this.userStore.setRoleForStore(tokenPayload.role);
        // Login successful
        console.log('Login successful', response);
        // Redirect to the desired page
        this.router.navigate(['/home']);
      },
      (error) => {
        // Login failed
        console.error('Login failed', error);
        // Display an error message or perform additional actions
        this.snackBar.open('Login failed. Please check your credentials.', 'Close', {
          duration: 10000, // Duration in milliseconds
        });
      }
    );
  }
}
