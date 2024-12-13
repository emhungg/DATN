import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { OrdePageComponent } from './orde-page/orde-page.component';

const routes: Routes = [
  {
    path: '', component: OrdePageComponent, data: {breadcrumb: 'Order'}
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class OrderManagerRoutingModule { }
