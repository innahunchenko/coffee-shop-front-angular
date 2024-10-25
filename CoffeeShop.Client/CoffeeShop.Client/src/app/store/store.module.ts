import { RouterModule } from "@angular/router";
import { NgModule } from "@angular/core";
import { StoreComponent } from "./store.component";
import { HeaderModule } from "../header/header.module";
import { CommonModule } from "@angular/common";
import { ProductListComponent } from "../catalog/productList.component";
import { CartComponent } from "../cart/cart.component";
import { MenuComponent } from "../header/menu.component.ts";
import { CheckoutComponent } from "../checkout/checkout.component";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { HTTP_INTERCEPTORS } from "@angular/common/http";
import { CredentialsInterceptor } from "../services/credentials.interceptor";
import { OrderConfirmationComponent } from "../checkout/order-confirmation.component";

@NgModule({
  declarations: [
    StoreComponent,
    CartComponent,
    MenuComponent,
    CheckoutComponent,
    OrderConfirmationComponent,
    ProductListComponent
  ],
  imports: [
    HeaderModule,
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
