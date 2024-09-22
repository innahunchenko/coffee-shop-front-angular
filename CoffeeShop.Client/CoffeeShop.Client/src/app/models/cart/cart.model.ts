import { ProductSelection } from "../../models/cart/productSelection.model";
import { CartRepository } from "../../services/cart/cartRepository";

export class Cart {
  constructor(private repository: CartRepository) {
    

  }

  selections: ProductSelection[] = [];
  totalPrice: number = 0;

  update() {
    throw new Error("Method not implemented.");
  }
}
