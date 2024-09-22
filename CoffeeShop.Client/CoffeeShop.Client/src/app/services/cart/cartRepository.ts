import { Cart } from "../../models/cart/cart.model";

export class CartRepository {
  cart: Cart;
  constructor(private http: HttpClient) { }

  getCart(): Observable<T> {
    console.log(dataType);
    console.log(sessionUrl);
    return this.http.get<T>(`${sessionUrl}/${dataType}`);
  }
}
