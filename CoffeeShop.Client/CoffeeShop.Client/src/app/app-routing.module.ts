import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CheckoutComponent } from './checkout/checkout.component';
import { OrderConfirmationComponent } from './checkout/order-confirmation.component';
import { StoreRoutingModule } from './store/store-routing.module';


const routes: Routes = [
  {
    path: "checkout",
    component: CheckoutComponent
  },
  {
    path: "order-confirmation",
    component: OrderConfirmationComponent
  },
  {
    path: "**",
    redirectTo: "",
    pathMatch: "full"
  }
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes),
    StoreRoutingModule
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }

