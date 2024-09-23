import { RouterModule } from "@angular/router";
import { NgModule } from "@angular/core";
import { StoreComponent } from "./store.component";
import { HeaderModule } from "../header/header.module";
import { CommonModule } from "@angular/common";
import { ProductListComponent } from "../catalog/productList.component";
import { CartComponent } from "../cart/cart.component";
import { MenuComponent } from "../header/menu.component.ts";

@NgModule({
  declarations: [
    StoreComponent,
    CartComponent,
    MenuComponent,
    ProductListComponent
  ],
  imports: [
    HeaderModule,
    //BrowserModule
    //BrowserAnimationsModule,
    //MatMenuModule,
    //MatButtonModule,
    RouterModule,
    CommonModule
  ],
  exports: [
    StoreComponent
  ]
})
export class StoreModule {
}
