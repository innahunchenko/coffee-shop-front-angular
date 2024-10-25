import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { forkJoin } from 'rxjs';
import { CartService } from '../services/cart/cart.service';
import { CatalogRepository } from '../services/catalog/catalog.repository';

@Component({
  selector: 'app-store',
  templateUrl: './store.component.html'
})
export class StoreComponent implements OnInit {
  isLoading = true;

  constructor(
    public cartService: CartService,
    private catalogRepository: CatalogRepository,
  ) { }

  ngOnInit(): void {
    this.loadData();
  }

  private loadData(): void {
    this.isLoading = true;

    const cart$ = this.cartService.loadCart();
    const products$ = this.catalogRepository.loadProducts();

    forkJoin([cart$, products$]).subscribe(
      () => {
        this.isLoading = false;
        console.log('Data loaded in store');
      },
      (error) => {
        console.error('Data loading error:', error);
        this.isLoading = false;  
      }
    );
  }
}
