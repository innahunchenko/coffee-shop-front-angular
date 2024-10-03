import { HttpClient } from "@angular/common/http";
import { Cart } from "../../models/cart/cart.model";
import { Observable } from "rxjs";
import { Injectable } from "@angular/core";
import { ProductSelection } from "../../models/cart/productSelection.model";

const API_BASE_URL = 'https://localhost:7075/cart';
const cartUrl = `${API_BASE_URL}/shopping-cart`;

@Injectable()
export class CartRepository {
  cart: Cart = new Cart();
  constructor(private http: HttpClient) { }

  getCart(): Observable<Cart> {
    return this.http.get<Cart>(cartUrl, { withCredentials: true });
  }

  storeCart(selections: ProductSelection[]): Observable<Cart> {
    return this.http.post<Cart>(`${cartUrl}/add`, selections, { withCredentials: true });
  }
}
