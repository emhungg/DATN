import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DienThoaiHomeComponent } from './dien-thoai-home/dien-thoai-home.component';
import { DienThoaiCateroyIdComponent } from './dien-thoai-cateroy-id/dien-thoai-cateroy-id.component';

const routes: Routes = [
  {
    path: '', component: DienThoaiHomeComponent, data: { breadcrumb: 'Tất cả' }
  },
  {
    path: ':id', component: DienThoaiCateroyIdComponent, data: { breadcrumb: { alias: 'filterType' } }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DienThoaiRoutingModule { }
