import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppleHomeComponent } from './apple-home/apple-home.component';

const routes: Routes = [
  {
    path: '', component: AppleHomeComponent,
    data: { breadcrumb: { skip: true, alias: 'skipApple' } }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AppleRoutingModule { }
