import { Injectable } from "@angular/core";
import { Cart } from "../../models/cart/cart.model";
import { CartRepository } from "./cart.repository";
import { ProductSelection } from "../../models/cart/productSelection.model";
import { BehaviorSubject, Observable, catchError, of, tap } from "rxjs";
import { CartCheckout } from "../../models/cart/cart-checkout.model";

@Injectable()
export class CartService {
  cart: Cart = new Cart();
  private cartSubject = new BehaviorSubject<Cart | null>(null);
  cart$ = this.cartSubject.asObservable();

  constructor(private repository: CartRepository) {
  }
  
  public loadCart(): Observable<Cart> {
    return this.repository.getCart().pipe(
      tap(cartData => {
        if (cartData) {
          this.cart.selections = cartData.selections;
          this.cart.totalPrice = cartData.totalPrice;
          this.cartSubject.next(cartData);
        } else {
          this.cartSubject.next(null);
        }
      }),
      catchError(error => {
        console.error('Error loading cart:', error);
        this.cartSubject.next(null);
        return of(new Cart());  
      })
    );
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
    this.repository.storeCart(this.cart.selections).subscribe(response => {
      this.cart = response;
      this.cartSubject.next(response);
      console.log('total price: ' + this.cart.totalPrice);
    });
  }

  removeProductFromCart(productId: string): void {
    this.cart.selections = this.cart.selections.filter(selection => selection.productId !== productId);
    this.updateCart();
  }

  clearCart(): void {
    this.cart.selections = [];
    this.updateCart(); 
  }

  increaseQuantity(productId: string): void {
    const selection = this.cart.selections.find(s => s.productId === productId);
    if (selection) {
      selection.quantity += 1; 
      this.updateCart(); 
    }
  }

  decreaseQuantity(productId: string): void {
    const selection = this.cart.selections.find(s => s.productId === productId);
    if (selection && selection.quantity > 1) {
      selection.quantity -= 1;
      this.updateCart();
    }
  }

  checkoutCart(checkoutData: CartCheckout): Observable<any> {
    return this.repository.checkoutCart(checkoutData);
  }

  storeCheckoutSessionData<T>(dataType: string, data: T): void {
    this.repository.storeSessionData(dataType, data);
  }

  getCheckoutSessionData<T>(dataType: string): Observable<T> {
    return this.repository.getSessionData<T>(dataType);
  }
}
