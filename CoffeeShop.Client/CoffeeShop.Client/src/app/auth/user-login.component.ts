import { Component } from "@angular/core";
import { FormBuilder, FormGroup } from "@angular/forms";
import { Router } from "@angular/router";
import { AuthService } from "../services/auth/auth.service";
import { User } from "../models/auth/user.interface";

@Component({
  selector: 'app-user-login',
  templateUrl: './user-login.component.html',
  styleUrls: ['./auth.component.css']
})
export class UserLoginComponent {
  loginForm: FormGroup;
  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router) {

    this.loginForm = this.fb.group({
      userName: [''],
      password: ['']
    });
  }

  onLogin() {
    const userData: User = this.loginForm.value;
    this.authService.login(userData).subscribe({
      next: () => {
        //this.router.navigate(['/']);
      },
      error: (errorResponse) => {
        if (errorResponse.error.errors) {
          const validationErrors = errorResponse.error.errors;
          this.handleValidationErrors(validationErrors);
        }
      }
    });
  }

  handleValidationErrors(validationErrors: any) {
    Object.keys(validationErrors).forEach(field => {
      if (this.loginForm.controls[field]) {
        this.loginForm.controls[field].setErrors({ serverError: validationErrors[field] });
      }
    });
  }

  goToRegister() {
    this.router.navigate(['/user/register']);
  }
}
