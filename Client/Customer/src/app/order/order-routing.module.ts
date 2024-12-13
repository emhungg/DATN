import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { OrderActionComponent } from './order-action/order-action.component';
import { OrderDoneComponent } from './order-done/order-done.component';

const routes: Routes = [
  {
    path: '', component: OrderActionComponent, data: { breadcrumb: { skip: true } }
  },
  {
    path: 'return', component: OrderDoneComponent, data: { breadcrumb: { skip: true } }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class OrderRoutingModule { }
