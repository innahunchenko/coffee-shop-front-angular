import { Injectable } from "@angular/core";
import { Cart } from "../../models/cart/cart.model";
import { CartRepository } from "./cartRepository";
import { ProductSelection } from "../../models/cart/productSelection.model";

@Injectable()
export class CartService {
  cart: Cart = new Cart(); 

  constructor(private repository: CartRepository) {
    this.loadCart();
  }

  private loadCart(): void {
    this.repository.getCart().subscribe(cartData => {
      if (cartData) {
        this.cart.selections = cartData.selections;
        this.cart.totalPrice = cartData.totalPrice;
      }
    });
  }

  addProductToCart(productSelection: ProductSelection): void {
    if (!this.cart.selections) {
      this.cart.selections = [];
    }

    const existingProduct = this.cart.selections
      .find(selection => selection.productId === productSelection.productId);

    if (existingProduct) {
      existingProduct.quantity = (existingProduct.quantity || 0) + (productSelection.quantity || 1);
    } else {
      this.cart.selections.push(productSelection);
    }

    this.updateCart();
  }

  private updateCart(): void {
    //this.cart.totalPrice = this.cart.selections.reduce((sum, selection) =>
    //  sum + (selection.price * selection.quantity), 0);
    this.repository.storeCart(this.cart.selections);
    this.cart = this.repository.cart;
  }

  removeProductFromCart(productId: string): void {
    this.cart.selections = this.cart.selections.filter(selection => selection.productId !== productId);
    this.updateCart();
  }

  clearCart(): void {
    this.cart = new Cart(); 
  }
}
