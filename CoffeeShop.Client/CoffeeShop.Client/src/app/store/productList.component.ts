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
  pageNumber = 1;
  maxPagesToShow = 3;
  currentRangeStart: number = 1;
  totalPages = 0;
  products: PaginatedList<Product> = new PaginatedList<Product>();
  productsSubscription!: Subscription;

  constructor(public repo: Repository) {
    this.repo.loadProducts();
  }

  ngOnInit() {
    this.productsSubscription = this.repo.products$.subscribe(products => {
      if (products) {
        this.products = products;

        if (this.totalPages !== this.products.totalPages && this.totalPages !== 0) {
          this.pageNumber = 1;
          this.currentRangeStart = 1;
          this.repo.setPageNumber(this.pageNumber);
          this.repo.loadProducts();
        }

        this.totalPages = products.totalPages;
      }
      else {
        this.pageNumber = 1;
        this.currentRangeStart = 1;
        this.repo.setPageNumber(this.pageNumber);
        this.repo.loadProducts();
      }
    });
  }

  ngOnDestroy() {
    if (this.productsSubscription) {
      this.productsSubscription.unsubscribe();
    }
  }

  goToPage(page: number) {
    if (page >= 1 && page <= this.products.totalPages) {
      this.pageNumber = page;
      this.repo.setPageNumber(this.pageNumber);
      this.repo.loadProducts();
    }
  }

  previousSetOfPages() {
    if (this.currentRangeStart > 1) {
      // Move to the previous range of pages, ensuring it does not go below 1
      this.currentRangeStart = Math.max(this.currentRangeStart - this.maxPagesToShow, 1);
      this.pageNumber = this.currentRangeStart + this.maxPagesToShow - 1;
      this.repo.setPageNumber(this.pageNumber);
      this.repo.loadProducts();
    }
  }

  nextSetOfPages() {
    // Calculate the last page in the current range
    const lastPageInRange = this.currentRangeStart + this.maxPagesToShow - 1;

    if (lastPageInRange < this.products.totalPages) {
      const newRangeStart = lastPageInRange + 1;

      // Ensure the new range does not exceed total pages
      this.currentRangeStart = Math.min(newRangeStart, this.products.totalPages - this.maxPagesToShow + 1);

      if (this.currentRangeStart < lastPageInRange) {
        this.currentRangeStart = newRangeStart;
      }

      this.pageNumber = this.currentRangeStart;
      this.repo.setPageNumber(this.pageNumber);
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
