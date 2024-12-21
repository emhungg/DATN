import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TabletListComponent } from './tablet-list/tablet-list.component';
import { TabletCategoryComponent } from './tablet-category/tablet-category.component';

const routes: Routes = [
  {
    path: '', component: TabletListComponent, data: { breadcrumb: 'Tất cả' }
  },
  {
    path: ':id', component: TabletCategoryComponent, data: { breadcrumb: { alias: 'filterType' } }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TabletRoutingModule { }
