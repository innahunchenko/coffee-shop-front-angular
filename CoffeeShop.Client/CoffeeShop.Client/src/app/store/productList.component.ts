import { Component } from "@angular/core";
import { Repository } from "../services/repository";
@Component({
  selector: "product-list",
  templateUrl: "productList.component.html"
})
export class ProductListComponent {
  pageNumber = 1;
  totalPages = 20; // Задайте общее количество страниц
  maxPagesToShow = 3; // Максимальное количество страниц в одном наборе
  pagesPerBatch = 3; // Количество страниц, отображаемых в каждом наборе (батче)
  currentPageBatch = 0; // Индекс текущего набора страниц
  maxPageBatches: number; // Общее количество наборов страниц

  constructor(public repo: Repository) {
    this.maxPageBatches = Math.ceil(this.totalPages / (this.maxPagesToShow * this.pagesPerBatch));
  }
  
  getPageNumbers(): number[] {
    const pages = [];
    const totalPages = this.repo.products.totalPages;
    const pagesPerGroup = this.maxPagesToShow; // Количество страниц, которые нужно показать в одной группе
    const currentGroup = Math.floor((this.pageNumber - 1) / pagesPerGroup); // Определение текущей группы страниц
    const startPage = currentGroup * pagesPerGroup + 1; // Начальная страница в текущей группе
    const endPage = Math.min(totalPages, startPage + pagesPerGroup - 1); // Конечная страница в текущей группе

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
