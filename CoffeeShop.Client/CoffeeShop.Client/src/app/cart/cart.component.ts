import { Component, OnInit } from '@angular/core';
import { CartService } from '../services/cart/cart.service';
import { Cart } from '../models/cart/cart.model';
import { Subscription } from 'rxjs';

@Component({
  selector: 'cart',
  templateUrl: './cart.component.html'
})
export class CartComponent implements OnInit {
  cart: Cart | null = null;
  cartSubscription!: Subscription;
  constructor(public cartService: CartService) { }

  ngOnInit(): void {
    this.cartSubscription = this.cartService.cart$.subscribe(cart => {
      this.cart = cart;
    });
  }

  getTotalPrice() {
    return this.cartService.cart.totalPrice;
  }

  getCartItemsCount() {
    return this.cartService.cart.selections.length;
  }

  removeProductFromCart(productId: string) {
    this.cartService.removeProductFromCart(productId);
  }

  clearCart() {
    this.cartService.clearCart();
  }

  increaseQuantity(productId: string) {
    this.cartService.increaseQuantity(productId);
  }

  decreaseQuantity(productId: string) {
    this.cartService.decreaseQuantity(productId);
  }
}
