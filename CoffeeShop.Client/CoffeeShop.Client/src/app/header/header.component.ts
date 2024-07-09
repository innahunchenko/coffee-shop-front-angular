import { Component } from '@angular/core';
import { Repository } from '../services/repository';
import { Category } from '../models/category.model';
import { NavigationService } from '../services/navigation.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html'
})
export class HeaderComponent {
  selectedCategory: string;
  selectedSubcategory: string = "";
  categories: Category[] = [];

  constructor(public repository: Repository, private navigation: NavigationService) {
    this.selectedCategory = this.navigation.currentCategory === "" ? "All categories" : this.navigation.currentCategory;
  }

  setCurrentCategory(category: string, subcategory: string | ""): void {
    this.selectedCategory = category === "All categories" ? "" : category;
    this.selectedSubcategory = subcategory;
    this.navigation.currentCategory = this.selectedCategory;
    this.navigation.currentSubcategory = subcategory;
  }
}
