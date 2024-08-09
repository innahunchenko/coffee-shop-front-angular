import { Product } from "../models/product.model";
import { Injectable } from "@angular/core";
import { HttpClient, HttpParams } from "@angular/common/http";
import { BehaviorSubject, Observable } from "rxjs";
import { catchError } from "rxjs/operators";
import { Category } from "../models/category.model";
import { PaginatedList } from "../models/paginatedList.model";

const API_BASE_URL = 'http://localhost:5000/api';
const productsUrl = `${API_BASE_URL}/products`;
const categoriesUrl = `${API_BASE_URL}/categories`;
const byCategory = 'category';
const bySubcategory = 'subcategory';
const byName = 'productName';

@Injectable()
export class Repository {
  private productsSubject = new BehaviorSubject<PaginatedList<Product>>(new PaginatedList());
  products$: Observable<PaginatedList<Product>> = this.productsSubject.asObservable();
  categories: Category[] = [];
  pageNumber: number = 1;
  pageSize: number = 10;
  filterValue: string = "";
  filterType: string = "";

  constructor(private http: HttpClient) {
  }

  public loadProducts(): void {
    let params = new HttpParams()
      .set('pageNumber', this.pageNumber.toString())
      .set('pageSize', this.pageSize.toString());

    params = params.set(this.filterType, this.filterValue);

    let url = productsUrl;
    if (this.filterType) {
      url += `/${this.filterType}`;
    }

    this.http.get<PaginatedList<Product>>(url, { params }).pipe(
      catchError(error => {
        console.error(`Error loading products by ${this.filterType || 'all'}:`, error);
        throw error;
      })
    ).subscribe(
      products => {
        this.productsSubject.next(products);
        console.log(`Products loaded by ${this.filterType || 'all'}:`, products);
      }
    );
  }

  setPageNumber(pageNumber: number) {
    this.pageNumber = pageNumber;
  }

  getProductsByCategory(category: string): void {
    this.filterType = byCategory;
    this.filterValue = category;
    this.loadProducts();
  }

  getProductsBySubcategory(subcategory: string): void {
    this.filterType = bySubcategory;
    this.filterValue = subcategory;
    this.loadProducts();
  }

  getProductsByName(name: string): void {
    this.filterType = byName;
    this.filterValue = name;
    this.loadProducts();
  }

  getAllProducts(): void {
    this.filterType = "";
    this.filterValue = "";
    this.loadProducts();
  }

  getCategories() {
    console.log(`${categoriesUrl}`);
    this.http.get<Category[]>(`${categoriesUrl}`).subscribe(categories => this.categories = categories);
  }
}
