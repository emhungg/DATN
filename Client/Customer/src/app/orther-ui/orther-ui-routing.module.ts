import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { OrtherUiComponent } from './orther-ui/orther-ui.component';

const routes: Routes = [
  {
    path: ':id', component: OrtherUiComponent, data: { breadcrumb: { alias: 'true', skip: true } }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class OrtherUIRoutingModule { }
