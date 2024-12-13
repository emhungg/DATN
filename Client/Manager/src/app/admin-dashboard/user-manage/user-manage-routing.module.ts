import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UserCheckoutComponent } from './user-checkout/user-checkout.component';

const routes: Routes = [
  {
    path: '', component: UserCheckoutComponent, data: { breadcrumb: 'User Manage' }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class UserManageRoutingModule { }
