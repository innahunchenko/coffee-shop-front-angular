import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { StoreComponent } from './store/store.component';
import { CheckoutComponent } from './checkout/checkout.component';
import { OrderConfirmationComponent } from './checkout/order-confirmation.component';
import { UserRegisterComponent } from './auth/user-register.component';
import { UserLoginComponent } from './auth/user-login.component';
import { StoreRoutingModule } from './store/store-routing.module';

const routes: Routes = [
  { path: "checkout", component: CheckoutComponent },
  { path: "order-confirmation", component: OrderConfirmationComponent },
  { path: "user/register", component: UserRegisterComponent },
  { path: "user/login", component: UserLoginComponent },
  { path: "**", redirectTo: "", pathMatch: "full" }
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes),
    StoreRoutingModule
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }

