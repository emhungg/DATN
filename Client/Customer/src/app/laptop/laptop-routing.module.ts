import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LaptopListComponent } from './laptop-list/laptop-list.component';
import { LaptopCategoryComponent } from './laptop-category/laptop-category.component';

const routes: Routes = [{
  path: '', component: LaptopListComponent, data: { breadcrumb: 'Tất cả' }
}, {
  path: ':id', component: LaptopCategoryComponent, data: { breadcrumb: { alias: 'filterType' } }
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class LaptopRoutingModule { }
