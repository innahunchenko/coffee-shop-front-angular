import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProductListComponent } from '../catalog/productList.component';
import { ManageCatalogComponent } from '../management/manage-catalog.component';
import { StoreComponent } from './store.component';

const routes: Routes = [
  {
    path: '',
    component: StoreComponent,
    children: [
      { path: '', component: ProductListComponent },  
      { path: 'manage-catalog', component: ManageCatalogComponent }  
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)], 
  exports: [RouterModule]
})
export class StoreRoutingModule { }
