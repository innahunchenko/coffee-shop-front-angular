import { Product } from "../models/product.model";
import { Injectable } from "@angular/core";
import { HttpClient, HttpParams } from "@angular/common/http";
import { catchError } from "rxjs";
import { Category } from "../models/category.model";
import { Filter } from "../models/filter.model";
import { PaginatedList } from "../models/paginatedList.model";

const API_BASE_URL = 'http://localhost:5000/api';
const productsUrl = `${API_BASE_URL}/products`;
const categoriesUrl = `${API_BASE_URL}/categories`;

@Injectable()
export class Repository {
  products: PaginatedList<Product> = new PaginatedList();
  categories: Category[] = [];
  filter: Filter = new Filter();
  pageNumber: number = 1;
  pageSize: number = 10;
  constructor(private http: HttpClient) {
    this.getCategories();
  }

  getProducts(pageNumber: number = this.pageNumber, pageSize: number = this.pageSize): void {
    let params = new HttpParams();

    if (this.filter) {
      Object.keys(this.filter).forEach(key => {
        const value = this.filter[key];
        if (value !== undefined && value !== null && value !== '') {
          params = params.set(key, value);
        }
      });
    }

    params = params.set('pageNumber', pageNumber.toString());
    params = params.set('pageSize', pageSize.toString());

    this.http.get<PaginatedList<Product>>(productsUrl, { params }).pipe(
      catchError(error => {
        console.error('Error loading products:', error);
        throw error;
      })
    ).subscribe(
      products => {
        this.products = products;
        this.products.totalPages = products.totalPages;
        this.products.totalItems = products.totalItems;
        this.products.pageIndex = products.pageIndex;
        console.log('Products loaded:', this.products, 'total products: ', this.products.totalItems);
        console.log('total pages: ', this.products.totalPages);
      },
      error => {
        console.error('Error loading products:', error);
      }
    );
  }
  
  getCategories() {
    console.log(`${categoriesUrl}`);
    this.http.get<Category[]>(`${categoriesUrl}`).subscribe(categories => this.categories = categories);
  }
}
