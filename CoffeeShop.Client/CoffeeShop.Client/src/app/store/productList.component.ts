import { Component, OnDestroy, OnInit } from "@angular/core";
import { Repository } from "../services/repository";
import { PaginatedList } from "../models/paginatedList.model";
import { Product } from "../models/product.model";
import { Subscription } from "rxjs";
@Component({
  selector: "product-list",
  templateUrl: "productList.component.html"
})
export class ProductListComponent implements OnInit, OnDestroy {
  maxPagesToShow = 3;
  currentRangeStart: number = 1;
  totalPages = 0;
  products: PaginatedList<Product> = new PaginatedList<Product>();
  productsSubscription!: Subscription;
  loadingSubscription!: Subscription;
  isLoading = true;

  constructor(public repo: Repository) {
    this.repo.loadProducts();
  }

  ngOnInit() {
    this.loadingSubscription = this.repo.loading$.subscribe(loading => {
      this.isLoading = loading;
    });

    this.productsSubscription = this.repo.products$.subscribe(products => {
      if (products) {
        this.products = products;
        this.isLoading = false;

        if (this.repo.pageNumber === 1) {
          this.currentRangeStart = 1;
        }

        this.totalPages = products.totalPages;

      } else {
        this.isLoading = false;
        this.repo.pageNumber = 1;
        this.currentRangeStart = 1;
        this.repo.loadProducts();
      }
    });
  }

  ngOnDestroy() {
    if (this.productsSubscription) {
      this.productsSubscription.unsubscribe();
    }
    if (this.loadingSubscription) {
      this.loadingSubscription.unsubscribe();
    }
  }

  goToPage(page: number) {
    if (page >= 1 && page <= this.products.totalPages) {
      this.repo.pageNumber = page;
      this.repo.loadProducts();
    }
  }

  previousSetOfPages() {
    if (this.currentRangeStart > 1) {
      this.currentRangeStart = Math.max(this.currentRangeStart - this.maxPagesToShow, 1);
      this.repo.pageNumber = this.currentRangeStart + this.maxPagesToShow - 1;
      this.repo.loadProducts();
    }
  }

  nextSetOfPages() {
    const lastPageInRange = this.currentRangeStart + this.maxPagesToShow - 1;

    if (lastPageInRange < this.products.totalPages) {
      const newRangeStart = lastPageInRange + 1;
      this.currentRangeStart = Math.min(newRangeStart, this.products.totalPages - this.maxPagesToShow + 1);

      if (this.currentRangeStart < lastPageInRange) {
        this.currentRangeStart = newRangeStart;
      }

      this.repo.pageNumber = this.currentRangeStart;
      this.repo.loadProducts();
    }
  }

  getPageNumbers(): number[] {
    const pages = [];
    const startPage = this.currentRangeStart;
    const endPage = Math.min(this.products.totalPages, startPage + this.maxPagesToShow - 1);

    for (let i = startPage; i <= endPage; i++) {
      pages.push(i);
    }

    return pages;
  }
}
