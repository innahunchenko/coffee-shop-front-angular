import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { StoreComponent } from './store/store.component';

const routes: Routes = [
  { path: "store", component: StoreComponent },
  { path: "", redirectTo: "/store", pathMatch: "full" },
  { path: "store/:category", component: StoreComponent },
  { path: "store/:category/:subcategory", component: StoreComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
