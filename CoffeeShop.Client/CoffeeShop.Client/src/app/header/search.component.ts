import { Component, ElementRef, ViewChild, AfterViewInit, OnInit } from '@angular/core';
import { Repository } from '../services/repository';
import { SearchStateService } from '../services/searchState.service';

@Component({
  selector: 'search',
  templateUrl: './search.component.html'
})
export class SearchComponent implements AfterViewInit, OnInit {
  search: string = "";

  @ViewChild('textInput', { static: false }) textInputRef!: ElementRef<HTMLInputElement>;
  @ViewChild('textInputOffcanvas', { static: false }) textInputOffcanvasRef!: ElementRef<HTMLInputElement>;

  constructor(public repository: Repository, private searchStateService: SearchStateService) { }

  ngOnInit(): void {
    this.setupOffcanvasListeners();
  }

  ngAfterViewInit(): void {
    this.updateInputValues();
  }

  updateInputValues(): void {
    this.search = this.searchStateService.getSearchTerm();
    if (this.textInputRef) {
      this.textInputRef.nativeElement.value = this.search;
    }
    if (this.textInputOffcanvasRef) {
      this.textInputOffcanvasRef.nativeElement.value = this.search;
    }
  }

  setupOffcanvasListeners(): void {
    const offcanvasElement = document.getElementById('offcanvasSearch');
    if (offcanvasElement) {
      offcanvasElement.addEventListener('hidden.bs.offcanvas', () => {
        this.updateInputValues();
      });
      offcanvasElement.addEventListener('shown.bs.offcanvas', () => {
        this.updateInputValues();
      });
    }
  }

  onInputChange(input: 'main' | 'offcanvas') {
    if (input === 'main' && this.textInputRef) {
      this.search = this.textInputRef.nativeElement.value;
    } else if (input === 'offcanvas' && this.textInputOffcanvasRef) {
      this.search = this.textInputOffcanvasRef.nativeElement.value;
    }
    this.searchStateService.setSearchTerm(this.search);
  }

  onSubmit() {
    this.repository.filter.search = this.searchStateService.getSearchTerm();
     this.repository.getProducts();
  }
}
