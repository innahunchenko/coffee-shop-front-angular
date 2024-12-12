import { Component, OnDestroy, OnInit } from "@angular/core";
import { CatalogRepository } from "../services/catalog/catalog.repository";
import { Subscription } from "rxjs";
import { PaginatedList } from "../models/catalog/paginatedList.model";
import { Product } from "../models/catalog/product.model";
import { CartService } from "../services/cart/cart.service";
import { ProductSelection } from "../models/cart/productSelection.model";
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

  constructor(public repo: CatalogRepository, public cartService: CartService) { }

  ngOnInit() {
    this.loadingSubscription = this.repo.loading$.subscribe(loading => {
      this.isLoading = loading;
    });

    this.productsSubscription = this.repo.products$.subscribe(products => {
      this.updateProductList(products);
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

  loadProducts(): void {
    this.isLoading = true;
    this.repo.loadProducts().subscribe(products => {
      this.updateProductList(products);
    });
  }

  private updateProductList(products: PaginatedList<Product>): void {
    this.products = products;
    this.isLoading = false; 
    this.totalPages = products.totalPages;

    if (this.repo.pageNumber === 1) {
      this.currentRangeStart = 1;
    }
  }

  goToPage(page: number) {
    if (page >= 1 && page <= this.products.totalPages) {
      this.repo.pageNumber = page;
      this.loadProducts();
    }
  }

  previousSetOfPages() {
    if (this.currentRangeStart > 1) {
      this.currentRangeStart = Math.max(this.currentRangeStart - this.maxPagesToShow, 1);
      this.repo.pageNumber = this.currentRangeStart + this.maxPagesToShow - 1;
      this.loadProducts();
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
      this.loadProducts();
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

  addProductToCart(productId?: string) {
    if (!productId) {
      return;
    }

    this.cartService.addProductToCart(new ProductSelection(productId));
  }
}
