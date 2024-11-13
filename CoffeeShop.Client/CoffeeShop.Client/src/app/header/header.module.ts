import { CommonModule } from "@angular/common";
import { HeaderComponent } from "./header.component";
import { SearchComponent } from "./search.component";
import { NgModule } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { AuthModalComponent } from "../auth/auth-modal.component";

@NgModule({
  declarations: [
    HeaderComponent,
    SearchComponent,
    AuthModalComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule
  ],
  exports: [
    HeaderComponent
  ]
})
export class HeaderModule { }
