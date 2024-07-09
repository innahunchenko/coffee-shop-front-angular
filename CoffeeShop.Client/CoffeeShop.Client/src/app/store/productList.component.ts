import { Component } from "@angular/core";
import { Repository } from "../services/repository";
@Component({
  selector: "product-list",
  templateUrl: "productList.component.html"
})
export class ProductListComponent {
  constructor(public repo: Repository) {
  }
}
