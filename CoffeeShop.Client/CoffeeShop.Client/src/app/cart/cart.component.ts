import { Component, OnInit } from '@angular/core';
import { CartService } from '../services/cart/cart.service';
import { Cart } from '../models/cart/cart.model';

@Component({
  selector: 'cart',
  templateUrl: './cart.component.html'
})
export class CartComponent implements OnInit {
  cart: Cart | null = null;
  constructor(public cartService: CartService) { }

  ngOnInit(): void {
    this.loadCart();
  }

  private loadCart(): void {
    this.cartService.loadCart().subscribe(cartData => {
      this.cart = cartData;
    });
  }

  getTotalPrice() {
    return this.cartService.cart.totalPrice;
  }

  getCartItemsCount() {
    return this.cartService.cart.selections.length;
  }
}
