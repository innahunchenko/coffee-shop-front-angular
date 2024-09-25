import { HttpClient } from "@angular/common/http";
import { Cart } from "../../models/cart/cart.model";
import { Observable } from "rxjs";
import { Injectable } from "@angular/core";
import { ProductSelection } from "../../models/cart/productSelection.model";

const API_BASE_URL = 'http://localhost:7005/cart';
const cartUrl = `${API_BASE_URL}/shopping-cart`;

@Injectable()
export class CartRepository {
  cart: Cart = new Cart();
  constructor(private http: HttpClient) { }

  getCart(): Observable<Cart> {
    return this.http.get<Cart>(cartUrl, { });
  }

  storeCart(selections: ProductSelection[]) {
    this.http.post<Cart>(cartUrl, selections).subscribe(response => this.cart = response)
  }
}
