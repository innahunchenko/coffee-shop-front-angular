import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html'
})
export class HeaderComponent {
  isModalVisible = false;

  constructor(private router: Router) { }

  @Input() totalPrice: number = 0;

  openModal() {
    this.isModalVisible = true;
  }

  closeModal() {
    this.isModalVisible = false;
  }

  goToLogin() {
    this.router.navigate(['/user/login']);
  }
}
