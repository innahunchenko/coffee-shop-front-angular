import { Component } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth/auth.service';
import { User } from '../models/auth/user.interface';

@Component({
  selector: 'app-user-register',
  templateUrl: './user-register.component.html',
  styleUrls: ['./auth.component.css']
})
export class UserRegisterComponent {
  registerForm: FormGroup;
  userData: User = {} as User;
  generalErrorMessages: string[] = [];

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router)
  {
    this.registerForm = this.fb.group({
      firstName: [''],
      lastName: [''],
      email: [''],
      userName: [''],
      phoneNumber: [''],
      password: [''],
      dateOfBirth: ['']
    });
  }

  onRegister() {
    const userData: User = this.registerForm.value;
    this.authService.register(userData).subscribe({
      next: () => {
        this.router.navigate(['/user/login']);
      },
      error: (errorResponse) => {
        this.generalErrorMessages = [];
        if (errorResponse.error.errors != null) {
          const validationErrors = errorResponse.error.errors;
          this.handleValidationErrors(validationErrors);
        }
        else {
          this.generalErrorMessages = errorResponse.error.map((err: any) => err.description);
        }
      }
    });
  }

  handleValidationErrors(validationErrors: any) {
    Object.keys(validationErrors).forEach(field => {
      if (this.registerForm.controls[field]) {
        this.registerForm.controls[field].setErrors({ serverError: validationErrors[field] });
      }
    });
  }
}
