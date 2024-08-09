import { NgModule } from "@angular/core";
import { HttpClientModule } from '@angular/common/http';
import { Repository } from "../services/repository";
import { SearchStateService } from "../services/searchState.service";

@NgModule({
  imports: [HttpClientModule],
  providers: [Repository, SearchStateService]
})
export class ModelModule { }
