import { HttpClient } from "@angular/common/http";
import { Cart } from "../../models/cart/cart.model";
import { Observable } from "rxjs";
import { Injectable } from "@angular/core";
import { ProductSelection } from "../../models/cart/productSelection.model";
import { CartCheckout } from "../../models/cart/cart-checkout.interface";
import { ENVIRONMENT } from "../../../environments/environment";

const apiGatewayUrl = ENVIRONMENT.apiGatewayUrl;
const baseUrl = `${apiGatewayUrl}/cart`;
const shippingCartUrl = `${baseUrl}/shopping-cart`;

@Injectable()
export class CartRepository {
  cart: Cart = new Cart();
  constructor(private http: HttpClient) { }

  getCart(): Observable<Cart> {
    return this.http.get<Cart>(shippingCartUrl);
  }

  storeCart(selections: ProductSelection[]): Observable<Cart> {
    return this.http.post<Cart>(`${shippingCartUrl}/add`, selections);
  }

  checkoutCart(checkoutData: CartCheckout): Observable<any> {
    return this.http.post(`${shippingCartUrl}/checkout`, checkoutData);
  }

  storeSessionData<T>(dataType: string, data: T) {
    return this.http.post(`${shippingCartUrl}/session/${dataType}`, data).subscribe();
  }

  getSessionData<T>(dataType: string): Observable<T> {
    return this.http.get<T>(`${shippingCartUrl}/session/${dataType}`);
  }


}
