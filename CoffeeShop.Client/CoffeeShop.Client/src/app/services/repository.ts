import { Product } from "../models/product.model";
import { Injectable } from "@angular/core";
import { HttpClient, HttpParams } from "@angular/common/http";
import { catchError } from "rxjs";
import { Category } from "../models/category.model";
import { Filter } from "../models/filter.model";

export const API_BASE_URL = 'https://localhost:8081/api';

const productsUrl = `${API_BASE_URL}/products`;

@Injectable()
export class Repository {
  products: Product[] = [];
  categories: Category[] = [];
  filter: Filter = new Filter();
  constructor(private http: HttpClient) {
    this.getCategories();
  }

  getProducts(): void {
    let params = new HttpParams();

    if (this.filter) {
      Object.keys(this.filter).forEach(key => {
        const value = this.filter[key];
        if (value !== undefined && value !== null && value !== '') {
          params = params.set(key, value);
        }
      });
    }

    this.http.get<Product[]>(productsUrl, { params }).pipe(
      catchError(error => {
        console.error('Error loading products:', error);
        throw error;
      })
    ).subscribe(
      products => {
        this.products = products;
        console.log('Products loaded:', this.products);
      },
      error => {
        console.error('Error loading products:', error);
      }
    );
  }
  
  getCategories() {
    console.log(`${productsUrl}/categories`);
    this.http.get<Category[]>(`${productsUrl}/categories`).subscribe(categories => this.categories = categories);
  }
}
