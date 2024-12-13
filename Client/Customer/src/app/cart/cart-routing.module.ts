import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CartProductComponent } from './cart-product/cart-product.component';

const routes: Routes = [
  {
    path: '', component: CartProductComponent, data: { breadcrumb: { skip: true } }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CartRoutingModule { }
