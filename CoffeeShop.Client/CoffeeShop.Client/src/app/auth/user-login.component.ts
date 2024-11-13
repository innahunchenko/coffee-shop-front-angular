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
  generalErrorMessages: string[] = [];
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
        this.router.navigate(['']);
      },
      error: (errorResponse) => {
        this.generalErrorMessages = [];
        this.generalErrorMessages = errorResponse.error.map((err: any) => err.description);
      }
    });
  }

  goToRegister() {
    this.router.navigate(['/user/register']);
  }
}
