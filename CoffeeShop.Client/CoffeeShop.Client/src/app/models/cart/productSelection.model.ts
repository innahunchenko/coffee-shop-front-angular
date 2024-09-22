import { Cart } from "../../services/cart/cartService";

export class ProductSelection {
  constructor(
    public cartService: Cart,
    public productId: string,
    public name?: string,
    public price?: number,
    private quantityValue?: number) { }

  get quantity() {
    return this.quantityValue ?? 0;
  }

  set quantity(newQuantity: number) {
    this.quantityValue = newQuantity;
    this.cartService.update();
  }
}
