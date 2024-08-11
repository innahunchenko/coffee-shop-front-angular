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
  pageSize: number = 10;
  filterValue: string = "";
  filterType: string = "";
  private _pageNumber: number = 1;

  constructor(private http: HttpClient) {
  }

  public loadProducts(): void {
    let params = new HttpParams()
      .set('pageNumber', this._pageNumber.toString())
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

  get pageNumber(): number {
    return this._pageNumber;
  }

  set pageNumber(value: number) {
    if (value >= 1) { 
      this._pageNumber = value;
    } else {
      console.error("Page number must be greater than or equal to 1.");
    }
  }

  getProductsByCategory(category: string): void {
    this.filterType = byCategory;
    this.filterValue = category;
    this.pageNumber = 1;
    this.loadProducts();
  }

  getProductsBySubcategory(subcategory: string): void {
    this.filterType = bySubcategory;
    this.filterValue = subcategory;
    this.pageNumber = 1;
    this.loadProducts();
  }

  getProductsByName(name: string): void {
    this.filterType = byName;
    this.filterValue = name;
    this.pageNumber = 1;
    this.loadProducts();
  }

  getAllProducts(): void {
    this.filterType = "";
    this.filterValue = "";
    this.pageNumber = 1;
    this.loadProducts();
  }

  getCategories() {
    console.log(`${categoriesUrl}`);
    this.http.get<Category[]>(`${categoriesUrl}`).subscribe(categories => this.categories = categories);
  }
}
