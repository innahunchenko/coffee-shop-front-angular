export class ProductSelection {
  constructor(
    public productId: string,
    public productName?: string,
    public quantity: number = 1,
    public price?: number
  ) { }
}
