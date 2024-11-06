import { NgModule } from "@angular/core";
import { HttpClientModule } from '@angular/common/http';
import { CatalogRepository } from "../services/catalog/catalog.repository";
import { SearchStateService } from "../services/searchState.service";
import { CartRepository } from "../services/cart/cart.repository";
import { CartService } from "../services/cart/cart.service";
import { AuthService } from "../services/auth/auth.service";

@NgModule({
  imports: [HttpClientModule],
  providers: [
    CatalogRepository,
    CartRepository,
    CartService,
    AuthService,
    SearchStateService]
})
export class ModelModule { }
