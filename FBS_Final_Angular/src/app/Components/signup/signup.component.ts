import { Component } from '@angular/core';
import { AuthService } from 'src/app/Services/auth.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { RegistrationModel } from 'src/app/Models/RegisterModel';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.scss']
})
export class SignupComponent {

  email: string = '';
  password: string = '';
  confirmPassword: string = '';
  emailValid: boolean = true;
  passwordsMatch: boolean = true;
  registerForm! :FormGroup;

  
  constructor(private fb:FormBuilder, private authService: AuthService, private router: Router, private snackBar: MatSnackBar) { 
    this.registerForm=this.fb.group({
      email:['',Validators.compose([Validators.required, Validators.email])],
      password:['',Validators.required],confirmPaswword:['',Validators.required]
    })
  }

  register() {
    // this.emailValid = this.validateEmail();
    // this.passwordsMatch = this.validatePasswords();

    if (true) {
      const userData = new RegistrationModel(this.email, this.password, this.confirmPassword);

      this.authService.signup(userData)
        .subscribe(response => {
            // Handle successful signup response, e.g., show a success message
            console.log('Signup successful', response);
            this.router.navigate(['login']);
          },
          error => {
            // Handle signup error, e.g., show an error message
            console.error('Signup error', error);
            this.snackBar.open('Sign Up failed. Please follow the format.', 'Close', {
              duration: 10000, // Duration in milliseconds
            });
          }
        );
    }
  }

  // validateEmail(): boolean {
  //   const emailRegex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;
  //   return emailRegex.test(this.email);
  // }

  // validatePasswords(): boolean {
  //   return this.password === this.confirmPassword;
  // }
}
