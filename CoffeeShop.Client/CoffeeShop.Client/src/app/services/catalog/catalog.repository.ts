import { Injectable } from "@angular/core";
import { HttpClient, HttpParams } from "@angular/common/http";
import { BehaviorSubject, Observable, of } from "rxjs";
import { catchError, tap } from "rxjs/operators";
import { PaginatedList } from "../../models/catalog/paginatedList.model";
import { Product } from "../../models/catalog/product.model";
import { Category } from "../../models/catalog/category.model";
import { ENVIRONMENT } from "../../../environments/environment";

const apiGatewayUrl = ENVIRONMENT.apiGatewayUrl;
const baseURL = `${apiGatewayUrl}/catalog`;
const productsUrl = `${baseURL}/products`;
const categoriesUrl = `${baseURL}/categories`;
const byCategory = 'category';
const bySubcategory = 'subcategory';
const byName = 'name';

@Injectable()
export class CatalogRepository {
  private productsSubject = new BehaviorSubject<PaginatedList<Product>>(new PaginatedList());
  products$: Observable<PaginatedList<Product>> = this.productsSubject.asObservable();

  private loadingSubject = new BehaviorSubject<boolean>(false);
  loading$: Observable<boolean> = this.loadingSubject.asObservable();

  categories: Category[] = [];
  pageSize: number = 8;
  filterValue: string = "";
  filterType: string = "";
  private _pageNumber: number = 1;

  constructor(private http: HttpClient) {
  }

  public loadProducts(): Observable<PaginatedList<Product>> {
    this.loadingSubject.next(true);
    let params = new HttpParams()
      .set('pageNumber', this._pageNumber.toString())
      .set('pageSize', this.pageSize.toString());

    if (this.filterType) {
      params = params.set(this.filterType, this.filterValue);
    }

    let url = productsUrl;
    if (this.filterType) {
      url += `/${this.filterType}`;
    }

    return this.http.get<PaginatedList<Product>>(url, { params }).pipe(
      tap(products => {
        this.productsSubject.next(products);
        this.loadingSubject.next(false);
      }),
      catchError(error => {
        this.productsSubject.next(new PaginatedList<Product>([], 0));
        this.loadingSubject.next(false);
        return of(new PaginatedList<Product>());
      })
    );
  }

  get pageNumber(): number {
    return this._pageNumber;
  }

  set pageNumber(value: number) {
    if (value >= 1) { 
      this._pageNumber = value;
    } 
  }

  getProductsByCategory(category: string): void {
    this.filterType = byCategory;
    this.filterValue = category;
    this.pageNumber = 1;
    this.loadProducts().subscribe();
  }

  getProductsBySubcategory(subcategory: string): void {
    this.filterType = bySubcategory;
    this.filterValue = subcategory;
    this.pageNumber = 1;
    this.loadProducts().subscribe();
  }

  getProductsByName(name: string): void {
    this.filterType = byName;
    this.filterValue = name;
    this.pageNumber = 1;
    this.loadProducts().subscribe();
  }

  getAllProducts(): void {
    this.filterType = "";
    this.filterValue = "";
    this.pageNumber = 1;
    this.loadProducts().subscribe();
  }

  getCategories() {
    this.http.get<Category[]>(`${categoriesUrl}`).subscribe(categories => this.categories = categories);
  }
}
