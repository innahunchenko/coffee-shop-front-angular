import { Injectable } from "@angular/core";
import { Repository } from "./repository";
import { ActivatedRoute, NavigationEnd, Router } from "@angular/router";
import { filter } from 'rxjs/operators';

@Injectable()
export class NavigationService {
  constructor(private repository: Repository, private router: Router, private active: ActivatedRoute) {
    router.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe(() => this.handleNavigationChange());
  }

  private handleNavigationChange() {
    const snapshot = this.active.firstChild?.snapshot;

    if (snapshot && snapshot.url.length > 0 && snapshot.url[0].path === 'store') {
      const params = snapshot.params;

      if (params['category'] !== undefined) {
        if (params['subcategory'] !== undefined) {
          this.updateCategorySubcategory(params['category'], params['subcategory']);
        } else {
          this.updateCategory(params['category']);
        }
      }
    }
    this.repository.getProducts();
  }

  private updateCategory(category: string) {
    this.repository.filter.category = category || '';
    this.repository.filter.subcategory = '';
  }

  private updateCategorySubcategory(category: string, subcategory: string) {
    this.repository.filter.category = category || '';
    this.repository.filter.subcategory = subcategory || '';
  }

  get currentCategory(): string {
    return this.repository.filter.category || "";
  }

  set currentCategory(category: string) {
    this.repository.filter.category = category;
    this.router.navigateByUrl(`/store/${(category || "").toLowerCase()}`);
  }

  get currentSubcategory(): string {
    return this.repository.filter.subcategory || "";
  }

  set currentSubcategory(subcategory: string) {
    this.repository.filter.subcategory = subcategory;

    let url = `/store/${(this.currentCategory).toLowerCase()}`;
    if (subcategory) {
      url += `/${subcategory.toLowerCase()}`;
    }

    this.router.navigateByUrl(url);
  }
}
