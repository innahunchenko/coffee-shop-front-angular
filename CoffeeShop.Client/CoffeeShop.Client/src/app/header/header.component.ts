import { Component } from '@angular/core';
import { CatalogRepository } from '../services/catalog/catalogRepository';
import { Category } from '../models/catalog/category.model';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html'
})
export class HeaderComponent {
  categories: Category[] = [];
  count: number = 0;

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
