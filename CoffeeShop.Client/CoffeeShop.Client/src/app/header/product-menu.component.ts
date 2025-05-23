import { Component } from "@angular/core";
import { Category } from "../models/catalog/category.model";
import { CatalogRepository } from "../services/catalog/catalog.repository";

@Component({
  selector: 'app-product-menu',
  templateUrl: './product-menu.component.html'
})
export class ProductMenuComponent {
  categories: Category[] = [];
  constructor(public repository: CatalogRepository) {
    this.repository.getCategories();
  }

  setCurrentCategory(category: string): void {
    this.repository.getProductsByCategory(category);
  }

  setCurrentSubcategory(subcategory: string): void {
    this.repository.getProductsBySubcategory(subcategory);
  }

  setAllCategories(): void {
    this.repository.getAllProducts();
  }
}
