import { NgModule } from "@angular/core";
import { HttpClientModule } from '@angular/common/http';
import { CatalogRepository } from "../services/catalog/catalog.repository";
import { SearchStateService } from "../services/searchState.service";
import { CartRepository } from "../services/cart/cart.repository";
import { CartService } from "../services/cart/cart.service";
import { AuthService } from "../services/auth/auth.service";
import { OrderRepository } from "../services/orders/order.repository";
import { OrderService } from "../services/orders/order.service";
import { CommonModule } from "@angular/common";
import { RoleGuard } from "../services/auth/role.guard";

@NgModule({
  imports: [HttpClientModule, CommonModule],
  providers: [
    CatalogRepository,
    CartRepository,
    CartService,
    AuthService,
    RoleGuard,
    SearchStateService,
    OrderRepository,
    OrderService
  ]
})
export class ModelModule { }
