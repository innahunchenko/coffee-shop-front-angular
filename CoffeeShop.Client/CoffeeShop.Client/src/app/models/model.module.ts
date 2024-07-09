import { NgModule } from "@angular/core";
import { HttpClientModule } from '@angular/common/http';
import { Repository } from "../services/repository";
import { NavigationService } from "../services/navigation.service";
import { SearchStateService } from "../services/searchState.service";

@NgModule({
  imports: [HttpClientModule],
  providers: [Repository, NavigationService, SearchStateService]
})
export class ModelModule { }
