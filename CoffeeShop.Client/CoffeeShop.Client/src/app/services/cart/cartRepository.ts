import { HttpClient } from "@angular/common/http";
import { Cart } from "../../models/cart/cart.model";
import { Observable } from "rxjs";
import { Injectable } from "@angular/core";

const API_BASE_URL = 'https://localhost:7070';
const cartUrl = `${API_BASE_URL}/shopping-cart`;

@Injectable()
export class CartRepository {
  cart: Cart = new Cart();
  constructor(private http: HttpClient) { }

  getCart(): Observable<Cart> {
    return this.http.get<Cart>(cartUrl, { });
  }

  storeCart(cart: Cart) {
    this.http.post<Cart>(cartUrl, cart).subscribe(response => this.cart = response)
  }
}
