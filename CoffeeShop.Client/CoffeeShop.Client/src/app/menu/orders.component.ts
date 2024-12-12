import { Component } from '@angular/core';
import { Order } from '../models/orders/order.interface';
import { OrderService } from '../services/orders/order.service';
import { AuthService } from '../services/auth/auth.service';

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.css'],
})
export class OrdersComponent {
  orders: Order[] = [];
  isLoading = true;
  isAdmin = false;

  constructor(private orderService: OrderService, private authService: AuthService) { }

  ngOnInit(): void {
    this.checkIfAdmin();

    this.orderService.getOrders().subscribe({
      next: (data) => {
        this.orders = data;
        this.isLoading = false;
      },
      error: (err) => {
        this.isLoading = false;
      }
    });
  }

  checkIfAdmin(): void {
    this.authService.isUserAdmin().subscribe({
      next: (isAdmin) => {
        this.isAdmin = isAdmin;
      },
      error: (err) => {
        console.error('Failed to determine if user is admin', err);
      },
    });
  }

  editOrder(order: Order): void {
    console.log('Edit order:', order);
  }
}
