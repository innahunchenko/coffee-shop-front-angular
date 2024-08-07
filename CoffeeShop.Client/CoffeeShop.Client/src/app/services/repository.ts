import { Product } from "../models/product.model";
import { Injectable } from "@angular/core";
import { HttpClient, HttpParams } from "@angular/common/http";
import { BehaviorSubject, Observable } from "rxjs";
import { catchError } from "rxjs/operators";
import { Category } from "../models/category.model";
import { Filter } from "../models/filter.model";
import { PaginatedList } from "../models/paginatedList.model";

const API_BASE_URL = 'http://localhost:5000/api';
const productsUrl = `${API_BASE_URL}/products`;
const categoriesUrl = `${API_BASE_URL}/categories`;

@Injectable()
export class Repository {
  private productsSubject = new BehaviorSubject<PaginatedList<Product>>(new PaginatedList());
  products$: Observable<PaginatedList<Product>> = this.productsSubject.asObservable();

  categories: Category[] = [];
  filter: Filter = new Filter();
  pageNumber: number = 1;
  pageSize: number = 10;

  constructor(private http: HttpClient) {
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

    params = params.set('pageNumber', this.pageNumber.toString());
    params = params.set('pageSize', this.pageSize.toString());

    this.http.get<PaginatedList<Product>>(productsUrl, { params }).pipe(
      catchError(error => {
        console.error('Error loading products:', error);
        throw error;
      })
    ).subscribe(
      products => {
        this.productsSubject.next(products); 
        console.log('Products loaded:', products, 'total products: ', products.totalItems);
        console.log('total pages: ', products.totalPages);
      },
      error => {
        console.error('Error loading products:', error);
      }
    );
  }

  setFilter(filter: Filter) {
    this.filter = filter;
    this.getProducts(); 
  }

  setPageNumber(pageNumber: number) {
    this.pageNumber = pageNumber;
    this.getProducts(); 
  }

  getCategories() {
    console.log(`${categoriesUrl}`);
    this.http.get<Category[]>(`${categoriesUrl}`).subscribe(categories => this.categories = categories);
  }
}
