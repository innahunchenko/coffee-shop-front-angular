import { Component } from '@angular/core';
import { Repository } from '../services/repository';
import { Category } from '../models/category.model';
import { Filter } from '../models/filter.model';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html'
})
export class HeaderComponent {
  categories: Category[] = [];
  count: number = 0;

  constructor(public repository: Repository) {
    this.repository.getCategories();
  }

  setCurrentCategory(category: string, subcategory: string | ""): void {
    const newFilter = new Filter();
    newFilter.category = category;
    newFilter.subcategory = subcategory;
    this.repository.setFilter(newFilter);
  }
}
