import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { WishListDestailsComponent } from './wish-list-destails/wish-list-destails.component';

const routes: Routes = [
  {
    path: 'data', component: WishListDestailsComponent, data: { breadcrumb: 'Sản phẩm yêu thích' }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class WishListRoutingModule { }
