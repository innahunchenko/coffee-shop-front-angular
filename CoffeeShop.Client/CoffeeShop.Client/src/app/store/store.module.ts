import { RouterModule } from "@angular/router";
import { FormsModule } from "@angular/forms";
import { MatButtonModule } from "@angular/material/button";
import { MatMenuModule } from "@angular/material/menu";
import { BrowserModule } from "@angular/platform-browser";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { NgModule } from "@angular/core";
import { StoreComponent } from "./store.component";
import { ProductListComponent } from "./productList.component";
import { HeaderModule } from "../header/header.module";
import { Repository } from "../services/repository";
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
