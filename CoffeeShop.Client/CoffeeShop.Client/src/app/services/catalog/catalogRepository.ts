import { Injectable } from "@angular/core";
import { HttpClient, HttpParams } from "@angular/common/http";
import { BehaviorSubject, Observable, of } from "rxjs";
import { catchError } from "rxjs/operators";
import { PaginatedList } from "../../models/catalog/paginatedList.model";
import { Product } from "../../models/catalog/product.model";
import { Category } from "../../models/catalog/category.model";

const API_BASE_URL = 'https://localhost:7070';
const productsUrl = `${API_BASE_URL}/products`;
const categoriesUrl = `${API_BASE_URL}/categories`;
const byCategory = 'category';
const bySubcategory = 'subcategory';
const byName = 'name';

@Injectable()
export class CatalogRepository {
  private productsSubject = new BehaviorSubject<PaginatedList<Product>>(new PaginatedList());
  private loadingSubject = new BehaviorSubject<boolean>(false);
  products$: Observable<PaginatedList<Product>> = this.productsSubject.asObservable();
  loading$: Observable<boolean> = this.loadingSubject.asObservable();
  categories: Category[] = [];
  pageSize: number = 8;
  filterValue: string = "";
  filterType: string = "";
  private _pageNumber: number = 1;

  constructor(private http: HttpClient) {
  }

  public loadProducts(): void {
    this.loadingSubject.next(true);

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
        this.loadingSubject.next(false);
        return of(new PaginatedList<Product>([], 0));
      })
    ).subscribe(
      products => {
        this.productsSubject.next(products);
        this.loadingSubject.next(false);
        console.log(`Products loaded by ${this.filterType || 'all'}:`, products);
      },
      () => {
        this.loadingSubject.next(false);
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
