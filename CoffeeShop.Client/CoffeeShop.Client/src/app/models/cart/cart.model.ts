import { ProductSelection } from "./productSelection.model";

export class Cart {
  constructor(
    public selections: ProductSelection[] = [],
    public totalPrice: number = 0
  ) { }

}
