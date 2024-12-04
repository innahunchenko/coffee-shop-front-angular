import { Component, EventEmitter, Output } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { AuthService } from '../services/auth/auth.service';
import { User } from '../models/auth/user.interface';
import { Router } from '@angular/router';

@Component({
  selector: 'app-auth-modal',
  templateUrl: './auth-modal.component.html',
  styleUrls: ['./auth-modal.component.css']
})
export class AuthModalComponent {
  loginForm: FormGroup;
  registerForm: FormGroup;
  forgotPasswordForm: FormGroup;
  generalErrorMessages: string[] = [];
  isSuccessRegister = false;
  isLoginMode = true;
  isForgotPasswordMode = false;
  isRegisterMode = false;

  @Output() close = new EventEmitter<void>();

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {
    this.loginForm = this.fb.group({
      userName: [''],
      password: ['']
    });
    this.registerForm = this.fb.group({
      email: [''],
      userName: [''],
      phoneNumber: [''],
      password: ['']
    });
    this.forgotPasswordForm = this.fb.group({
      email: ['']
    });
  }

  toggleMode(mode: 'login' | 'register' | 'forgotPassword') {
    this.isLoginMode = mode === 'login';
    this.isForgotPasswordMode = mode === 'forgotPassword';
    this.isRegisterMode = mode === 'register';
    this.generalErrorMessages = [];
    this.isSuccessRegister = false;
  }

  onLogin() {
    const userData: User = this.loginForm.value;
    this.authService.login(userData).subscribe({
      next: () => {
        this.router.navigate(['']);
        this.closeModal();  
      },
      error: (errorResponse) => {
        this.generalErrorMessages = errorResponse.error.map((err: any) => err.description);
      }
    });
  }

  onRegister() {
    const userData: User = this.registerForm.value;
    this.authService.register(userData).subscribe({
      next: () => {
        this.isSuccessRegister = true;
       // setTimeout(() => this.toggleMode('login'), 5000);
        //this.toggleMode('login');
      },
      error: (errorResponse) => {
        this.generalErrorMessages = [];
        const problemDetails = errorResponse.error;

        if (problemDetails.errors) {
          this.handleValidationErrors(problemDetails.errors);
        } else if (problemDetails.title || problemDetails.detail) {
          this.generalErrorMessages = [problemDetails.detail || problemDetails.title];
        } else if (Array.isArray(problemDetails)) {
          this.generalErrorMessages = problemDetails.map((err: any) => err.description);
        } else {
          this.generalErrorMessages = ['An unexpected error occurred. Please try again later.'];
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

  onForgotPassword() {
    const email = this.forgotPasswordForm.value.email;
    this.authService.sendPasswordResetEmail(email).subscribe({
      next: () => {
        alert('Password reset link sent to your email.');
        this.toggleMode('login');
      },
      error: (errorResponse) => {
        this.generalErrorMessages = errorResponse.error.map((err: any) => err.description);
      }
    });
  }

  closeModal() {
    this.close.emit();
  }
}
