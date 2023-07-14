import { Component } from '@angular/core';
import { AuthService } from 'src/app/Services/auth.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { RegistrationModel } from 'src/app/Models/RegisterModel';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-register-admin',
  templateUrl: './register-admin.component.html',
  styleUrls: ['./register-admin.component.css']
})
export class RegisterAdminComponent {
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

      this.authService.signupadmin(userData)
        .subscribe(response => {
            // Handle successful signup response, e.g., show a success message
            this.snackBar.open('New Admin Added Successfully.', 'Close', {
              duration: 10000, 
            });            
            this.router.navigate(['/admin']);
          },
          error => {
            // Handle signup error, e.g., show an error message
            console.error('Signup error', error);
            this.snackBar.open('Sign Up failed. Please follow the format.', 'Close', {
              duration: 10000, 
            });
          }
        );
    }
}
}