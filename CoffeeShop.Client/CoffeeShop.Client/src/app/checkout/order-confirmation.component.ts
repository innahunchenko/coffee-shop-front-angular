import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-order-confirmation',
  templateUrl: './order-confirmation.component.html',
  styleUrls: ['./order-confirmation.component.css']
})
export class OrderConfirmationComponent {
  constructor(private router: Router) { }

  goToStore() {
    this.router.navigate(['/store']);
  }
}
