import { Component } from "@angular/core";
import { Repository } from "../services/repository";
@Component({
  selector: "product-list",
  templateUrl: "productList.component.html"
})
export class ProductListComponent {
  pageNumber = 1;
  totalPages = 0; 
  maxPagesToShow = 3; 
  pagesPerBatch = 3; 
  currentRangeStart: number = 1;

  constructor(public repo: Repository) {
  }

  // Navigate to a specific page number
  goToPage(page: number) {
    // Check if the page number is within the valid range
    if (page >= 1 && page <= this.repo.products.totalPages) {
      this.pageNumber = page; // Set the current page number
      this.updateRangeStart(); // Update the range start based on the new page
    }
  }

  // Navigate to the previous set of pages
  previousPage() {
    // Check if there is a previous range to navigate to
    if (this.currentRangeStart > 1) {
      // Move to the previous range of pages, ensuring it does not go below 1
      this.currentRangeStart = Math.max(this.currentRangeStart - this.maxPagesToShow, 1);
      this.pageNumber = this.currentRangeStart; // Set the current page to the start of the new range
    }
  }

  // Navigate to the next set of pages
  nextPage() {
    // Calculate the last page in the current range
    const lastPageInRange = this.currentRangeStart + this.maxPagesToShow - 1;

    // Move to the next range of pages if there are more pages available
    if (lastPageInRange < this.repo.products.totalPages) {
      // Update the start page for the new range, ensuring it does not exceed total pages
      this.currentRangeStart = Math.min(lastPageInRange + 1, this.repo.products.totalPages - this.maxPagesToShow + 1);
      this.pageNumber = this.currentRangeStart; // Set the current page to the start of the new range
    }
  }

  // Get the list of page numbers to display
  getPageNumbers(): number[] {
    const pages = [];
    const totalPages = this.repo.products.totalPages;

    // Calculate the start and end page of the current range
    const startPage = this.currentRangeStart;
    const endPage = Math.min(totalPages, startPage + this.maxPagesToShow - 1);

    // Populate the pages array with the range of page numbers
    for (let i = startPage; i <= endPage; i++) {
      pages.push(i);
    }

    return pages;
  }

  // Update the start page of the range based on the current page number
  updateRangeStart() {
    // Calculate the new start page of the range, centering around the current page number
    this.currentRangeStart = Math.max(1, this.pageNumber - Math.floor(this.maxPagesToShow / 2));

    // Adjust the start page if the range exceeds the total number of pages
    if (this.currentRangeStart + this.maxPagesToShow - 1 > this.repo.products.totalPages) {
      this.currentRangeStart = Math.max(1, this.repo.products.totalPages - this.maxPagesToShow + 1);
    }
  }
}
