import { RouterModule } from "@angular/router";
import { NgModule } from "@angular/core";
import { StoreComponent } from "./store.component";
import { CommonModule } from "@angular/common";
import { ProductListComponent } from "../catalog/productList.component";
import { CartComponent } from "../cart/cart.component";
import { CheckoutComponent } from "../checkout/checkout.component";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { HTTP_INTERCEPTORS } from "@angular/common/http";
import { CredentialsInterceptor } from "../services/credentials.interceptor";
import { OrderConfirmationComponent } from "../checkout/order-confirmation.component";
import { ManageCatalogComponent } from "../menu/manage-catalog.component";
import { ProductMenuComponent } from "../header/product-menu.component";
import { HeaderComponent } from "../header/header.component";
import { SearchComponent } from "../header/search.component";
import { AuthModalComponent } from "../auth/auth-modal.component";
import { OrdersComponent } from "../menu/orders.component";

@NgModule({
  declarations: [
    StoreComponent,
    HeaderComponent,
    CartComponent,
    ProductMenuComponent,
    CheckoutComponent,
    OrderConfirmationComponent,
    ProductListComponent,
    ManageCatalogComponent,
    SearchComponent,
    AuthModalComponent,
    OrdersComponent
  ],
  imports: [
    RouterModule,
    CommonModule,
    ReactiveFormsModule,
    FormsModule
  ],
  exports: [
    StoreComponent
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: CredentialsInterceptor,
      multi: true  
    }
  ]
})
export class StoreModule {
}
