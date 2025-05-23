import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProductListComponent } from '../catalog/productList.component';
import { ManageCategoriesComponent } from '../menu/manage-categories.component';
import { StoreComponent } from './store.component';
import { OrdersComponent } from '../menu/orders.component';
import { RoleGuard } from '../services/auth/role.guard';

const routes: Routes = [
  {
    path: '',
    component: StoreComponent,
    children: [
      { path: '', component: ProductListComponent },
      {
        path: 'manage-categories',
        component: ManageCategoriesComponent,
        canActivate: [RoleGuard],
        data: { roles: ['ADMIN'] } 
      },
      {
        path: 'orders',
        component: OrdersComponent,
        canActivate: [RoleGuard],
        data: { roles: ['USER', 'ADMIN'] } 
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class StoreRoutingModule { }
