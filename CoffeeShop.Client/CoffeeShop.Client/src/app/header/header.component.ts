import { Component } from '@angular/core';
import { Repository } from '../services/repository';
import { Category } from '../models/category.model';
import { NavigationService } from '../services/navigation.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html'
})
export class HeaderComponent {
  categories: Category[] = [];
  count: number = 0;

  constructor(public repository: Repository, private navigation: NavigationService) {
  }

  setCurrentCategory(category: string, subcategory: string | ""): void {
    console.log(category);
    this.repository.filter.category = category;
    this.repository.filter.subcategory = subcategory;
    this.repository.getProducts();
  }
}
