import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { UserManageRoutingModule } from './user-manage-routing.module';
import { UserCheckoutComponent } from './user-checkout/user-checkout.component';
import { PrimeNgModule } from 'src/app/primeng.module';


@NgModule({
  declarations: [
    UserCheckoutComponent
  ],
  imports: [
    CommonModule,
    UserManageRoutingModule,
    PrimeNgModule
  ]
})
export class UserManageModule { }
