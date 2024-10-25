import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { StoreComponent } from './store/store.component';
import { CheckoutComponent } from './checkout/checkout.component';
import { OrderConfirmationComponent } from './checkout/order-confirmation.component';

const routes: Routes = [
  { path: 'checkout', component: CheckoutComponent },
  { path: "store", component: StoreComponent },
  { path: "", redirectTo: "/store", pathMatch: "full" },
  { path: "store/:category", component: StoreComponent },
  { path: "store/:category/:subcategory", component: StoreComponent },
  { path: 'order-confirmation', component: OrderConfirmationComponent }

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
