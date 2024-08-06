import { Component } from "@angular/core";
import { Repository } from "../services/repository";
@Component({
  selector: "product-list",
  templateUrl: "productList.component.html"
})
export class ProductListComponent {
  pageNumber = 1;
  totalPages = 20; 
  maxPagesToShow = 3; 
  pagesPerBatch = 3; 
  currentPageBatch = 0; 
  maxPageBatches: number; 

  constructor(public repo: Repository) {
    this.maxPageBatches = Math.ceil(this.totalPages / (this.maxPagesToShow * this.pagesPerBatch));
  }
  
  getPageNumbers(): number[] {
    const pages = [];
    const totalPages = this.repo.products.totalPages;
    const pagesPerGroup = this.maxPagesToShow;
    const currentGroup = Math.floor((this.pageNumber - 1) / pagesPerGroup); 
    const startPage = currentGroup * pagesPerGroup + 1; 
    const endPage = Math.min(totalPages, startPage + pagesPerGroup - 1);

    for (let i = startPage; i <= endPage; i++) {
      pages.push(i);
    }

    return pages;
  }

  previousPage() {
    if (this.pageNumber > 1) {
      this.pageNumber--;
    }
  }

  nextPage() {
    if (this.pageNumber < this.repo.products.totalPages) {
      this.pageNumber++;
    }
  }

  goToPage(page: number) {
    this.pageNumber = page;
  }
  
}
