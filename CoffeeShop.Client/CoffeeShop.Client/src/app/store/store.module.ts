import { RouterModule } from "@angular/router";
import { NgModule } from "@angular/core";
import { StoreComponent } from "./store.component";
import { ProductListComponent } from "./productList.component";
import { HeaderModule } from "../header/header.module";
import { CommonModule } from "@angular/common";

@NgModule({
  declarations: [
    StoreComponent,
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
