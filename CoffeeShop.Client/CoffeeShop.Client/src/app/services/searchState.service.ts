import { Injectable } from "@angular/core";

@Injectable({
  providedIn: "root",
})

export class SearchStateService {
  private search = "";

  getSearchTerm(): string {
    return this.search;
  }

  setSearchTerm(search: string): void {
    this.search = search;
  }
}
