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
  resetPasswordForm: FormGroup;
  generalErrorMessages: string[] = [];
  isSuccess = false;
  successfulMessage: string = "";
  isLoginMode = true;
  isForgotPasswordMode = false;
  isRegisterMode = false;
  isResetPasswordMode = false;
  email: string;
  token: string;

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

    this.resetPasswordForm = this.fb.group({
      password: [''],
      confirmPassword: ['']
    });

    const navigation = this.router.getCurrentNavigation();
    this.email = navigation?.extras?.state?.['email'] ?? '';
    this.token = navigation?.extras?.state?.['token'] ?? '';
  }

  toggleMode(mode: string, isSuccess: boolean) {
    this.isLoginMode = mode === 'login';
    this.isForgotPasswordMode = mode === 'forgotPassword';
    this.isRegisterMode = mode === 'register';
    this.isResetPasswordMode = mode === 'resetPassword';
    this.generalErrorMessages = [];
    this.isSuccess = isSuccess;
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
        this.successfulMessage = 'Registration successful';
        this.toggleMode('', true);
      },
      error: (errorResponse) => {
        this.handleValidationErrors(errorResponse.error, this.registerForm);
      }
    });
  }

  handleValidationErrors(problemDetails: any, formGroup: FormGroup) {
    this.generalErrorMessages = [];
    if (problemDetails.errors) {
      let hasFieldMatch = false;

      Object.keys(problemDetails.errors).forEach(field => {
        if (formGroup.controls[field]) {
          formGroup.controls[field].setErrors({ serverError: problemDetails.errors[field] });
          hasFieldMatch = true;
        }
      });

      if (!hasFieldMatch) {
        this.generalErrorMessages = Object.values(problemDetails.errors).map((value: any) =>
          Array.isArray(value) ? value.join(', ') : String(value)
        );
      }
    } else if (problemDetails.title || problemDetails.detail) {
      this.generalErrorMessages = [problemDetails.detail || problemDetails.title];
    } else if (Array.isArray(problemDetails)) {
      this.generalErrorMessages = problemDetails.map((err: any) => err.description);
    } else {
      this.generalErrorMessages = ['An unexpected error occurred. Please try again later.'];
    }
  }

  onForgotPassword() {
    const email = this.forgotPasswordForm.value.email;

    this.authService.forgotPassword(email).subscribe({
      next: (token) => {
        this.email = email;
        this.token = token;
        this.toggleMode('resetPassword', false);
      },
      error: (errorResponse) => {
        this.generalErrorMessages = errorResponse.error.errors || ['Failed email.'];
      }
    });
  }

  onResetPassword() {
    const { password, confirmPassword } = this.resetPasswordForm.value;

    this.authService.resetPassword(this.email, this.token, password, confirmPassword).subscribe({
      next: () => {
        this.successfulMessage = 'Password reset successfully';
        this.toggleMode('', true);
      },
      error: (errorResponse) => {
        this.handleValidationErrors(errorResponse.error, this.resetPasswordForm);
      }
    });
  }

  closeModal() {
    this.close.emit();
  }
}
