import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { OrderManagerRoutingModule } from './order-manager-routing.module';
import { OrdePageComponent } from './orde-page/orde-page.component';
import { PrimeNgModule } from 'src/app/primeng.module';


@NgModule({
  declarations: [
    OrdePageComponent
  ],
  imports: [
    CommonModule,
    OrderManagerRoutingModule,
    PrimeNgModule
  ]
})
export class OrderManagerModule { }
