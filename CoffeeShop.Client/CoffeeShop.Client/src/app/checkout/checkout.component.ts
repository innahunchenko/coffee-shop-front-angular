import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { CartCheckout } from '../models/cart/cart-checkout.model';
import { CartService } from '../services/cart/cart.service';
import { Cart } from '../models/cart/cart.model';
import { Subscription } from 'rxjs';
import { Router } from '@angular/router';

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

  constructor(private fb: FormBuilder, public cartService: CartService, private router: Router) {
    this.checkoutForm = this.fb.group({
      firstName: [''],
      lastName: [''],
      phoneNumber: [''],
      emailAddress: [''],
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
        console.log("session data: ", sessionData);
        if (sessionData) {
          this.checkoutData = sessionData;
          this.populateForm();
        }
      });
  }

  ngOnDestroy(): void {
    this.orderSubmitted = false;
  }

  loadCart() {
    this.cartService.loadCart().subscribe({
      next: (cart) => {
        this.cart = cart;
        console.log('Cart loaded:', cart);
      },
      error: (error) => {
        console.error('Error loading cart:', error);
      }
    });
  }

  populateForm() {
    this.checkoutForm.patchValue({
      firstName: this.checkoutData.firstName,
      lastName: this.checkoutData.lastName,
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
      console.log('Order cannot be submitted: Cart is empty or total price is 0.');
      this.router.navigate(['/store']);
      return;
    }

    const checkoutData: CartCheckout = this.checkoutForm.value;
    this.cartService.storeCheckoutSessionData('checkout', checkoutData);

    this.cartService.checkoutCart(checkoutData).subscribe({
      next: (response) => {
        console.log('Checkout successful!', response);
        this.orderSubmitted = true;
        this.loadCart();
        this.checkoutForm.reset();

        if (this.orderSubmitted) {
          this.router.navigate(['/order-confirmation']);
        }
      },
      error: (errorResponse) => {
        if (errorResponse.error.errors) {
          const validationErrors = errorResponse.error.errors;
          this.handleValidationErrors(validationErrors);
        }
      }
    });
  }

  setServerErrors(errors: any) {
    for (const field in errors) {
      if (this.checkoutForm.controls[field]) {
        this.checkoutForm.controls[field].setErrors({ serverError: errors[field] });
      }
    }
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
    this.router.navigate(['/store']);
  }
}
