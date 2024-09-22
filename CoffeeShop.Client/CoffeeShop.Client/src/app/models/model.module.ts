import { NgModule } from "@angular/core";
import { HttpClientModule } from '@angular/common/http';
import { CatalogRepository } from "../services/catalog/catalogRepository";
import { SearchStateService } from "../services/searchState.service";

@NgModule({
  imports: [HttpClientModule],
  providers: [CatalogRepository, SearchStateService]
})
export class ModelModule { }
