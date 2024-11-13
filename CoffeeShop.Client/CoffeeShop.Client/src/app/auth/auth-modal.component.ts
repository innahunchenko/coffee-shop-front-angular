import { Component, EventEmitter, Output } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth/auth.service';
import { User } from '../models/auth/user.interface';

@Component({
  selector: 'app-auth-modal',
  templateUrl: './auth-modal.component.html',
  styleUrls: ['./auth-modal.component.css']
})
export class AuthModalComponent {
  loginForm: FormGroup;
  registerForm: FormGroup;
  generalErrorMessages: string[] = [];
  isLoginMode = true;
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
      firstName: [''],
      lastName: [''],
      email: [''],
      userName: [''],
      phoneNumber: [''],
      password: [''],
      dateOfBirth: ['']
    });
  }

  toggleMode() {
    this.isLoginMode = !this.isLoginMode;
    this.generalErrorMessages = [];
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
        this.isLoginMode = true;  
      },
      error: (errorResponse) => {
        this.generalErrorMessages = [];
        if (errorResponse.error.errors) {
          this.handleValidationErrors(errorResponse.error.errors);
        } else {
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

  closeModal() {
    this.close.emit();
  }

  onRegisterClick() {
    this.close.emit();

    this.router.navigate(['/user/register']);
  }
}
