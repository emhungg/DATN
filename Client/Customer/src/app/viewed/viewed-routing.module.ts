import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ViewedProductComponent } from './viewed-product/viewed-product.component';

const routes: Routes = [
  {
    path: '', component: ViewedProductComponent, data: { breadcrumb: { skip: true } }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ViewedRoutingModule { }
