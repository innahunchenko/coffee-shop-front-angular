import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html'
})
export class HeaderComponent {

  constructor(private router: Router) { }

  @Input() totalPrice: number = 0;

  goToLogin() {
    this.router.navigate(['/user/login']);
  }
}
