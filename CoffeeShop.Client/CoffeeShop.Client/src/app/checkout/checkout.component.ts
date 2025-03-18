import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { CartCheckout } from '../models/cart/cart-checkout.interface';
import { CartService } from '../services/cart/cart.service';
import { Cart } from '../models/cart/cart.model';
import { Subscription } from 'rxjs';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth/auth.service';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.css']
})
export class CheckoutComponent implements OnInit {
  checkoutForm: FormGroup;
  cart: Cart | null = null;
  cartSubscription!: Subscription;
  checkoutData: CartCheckout = {} as CartCheckout;
  orderSubmitted: boolean = false;
  isLoggedIn: boolean = false;
  isLoading: boolean = false;

  constructor(
    private fb: FormBuilder,
    public cartService: CartService,
    private router: Router,
    private authService: AuthService
  ) {
    this.checkoutForm = this.fb.group({
      emailAddress: [''],
      phoneNumber: [''],
      addressLine: [''],
      country: [''],
      state: [''],
      zipCode: [''],
      cardName: [''],
      cardNumber: [''],
      expiration: [''],
      cvv: ['']
    });
  }

  ngOnInit(): void {
    this.orderSubmitted = false;
    this.cartSubscription = this.cartService.cart$.subscribe(cart => {
      this.cart = cart;
    });

    this.loadCart();

    this.cartService.getCheckoutSessionData<CartCheckout>('checkout')
      .subscribe(sessionData => {
        if (sessionData) {
          this.checkoutData = sessionData;
          this.populateForm();
        }
      });
    this.authService.isAuthenticated().subscribe();
    this.authService.isLoggedIn$.subscribe(isLoggedIn => {
      this.isLoggedIn = isLoggedIn;
    });
  }

  ngOnDestroy(): void {
    this.orderSubmitted = false;
  }

  loadCart() {
    this.cartService.loadCart().subscribe({
      next: (cart) => {
        this.cart = cart;
      },
      error: (error) => {
        console.error('Error loading cart:', error);
      }
    });
  }

  populateForm() {
    this.checkoutForm.patchValue({
      phoneNumber: this.checkoutData.phoneNumber,
      emailAddress: this.checkoutData.emailAddress,
      addressLine: this.checkoutData.addressLine,
      country: this.checkoutData.country,
      state: this.checkoutData.state,
      zipCode: this.checkoutData.zipCode,
      cardName: this.checkoutData.cardName,
      cardNumber: this.checkoutData.cardNumber,
      expiration: this.checkoutData.expiration,
      cvv: this.checkoutData.cvv
    });
  }

  submitOrder() {
    if (!this.cart || this.cart.totalPrice === 0) {
      this.router.navigate(['/']);
      return;
    }

    const checkoutData: CartCheckout = this.checkoutForm.value;
    this.cartService.storeCheckoutSessionData('checkout', checkoutData);

    this.isLoading = true;

    this.cartService.checkoutCart(checkoutData).subscribe({
      next: (response) => {
        this.orderSubmitted = true;
        this.loadCart();
        this.checkoutForm.reset();

        if (this.orderSubmitted) {
          this.router.navigate(['/order-confirmation']);
        }

        this.isLoading = false;
      },
      error: (errorResponse) => {
        if (errorResponse.error.errors) {
          const validationErrors = errorResponse.error.errors;
          this.handleValidationErrors(validationErrors);
        }

        this.isLoading = false;
      }
    });
  }

  handleValidationErrors(validationErrors: any) {
    Object.keys(validationErrors).forEach(field => {
      if (this.checkoutForm.controls[field]) {
        this.checkoutForm.controls[field].setErrors({ serverError: validationErrors[field] });
      }
    });
  }

  getCartItemsCount() {
    return this.cartService.cart.selections.length;
  }

  getTotalPrice() {
    return this.cartService.cart.totalPrice;
  }

  goToStore() {
    this.router.navigate(['']);
  }
}
