export class Category {
  constructor(
    public name: string,
    public subcategories?: string[] | null
  ) { }
}
