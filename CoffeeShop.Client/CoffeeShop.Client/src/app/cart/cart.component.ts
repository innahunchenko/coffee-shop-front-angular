import { Component } from '@angular/core';
import { CartService } from '../services/cart/cart.service';

@Component({
  selector: 'cart',
  templateUrl: './cart.component.html'
})
export class CartComponent {
  constructor(public cartService: CartService) { }

  getTotalPrice() {
    return this.cartService.cart.totalPrice;
  }

  getCartItemsCount() {
    return this.cartService.cart.selections.length;
  }
}
