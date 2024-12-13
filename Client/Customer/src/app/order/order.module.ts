import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { OrderRoutingModule } from './order-routing.module';
import { OrderActionComponent } from './order-action/order-action.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { OrderDoneComponent } from './order-done/order-done.component';
import { NgxSpinnerModule } from 'ngx-spinner';


@NgModule({
  declarations: [
    OrderActionComponent,
    OrderDoneComponent
  ],
  imports: [
    CommonModule,
    OrderRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    NgxSpinnerModule
  ]
})
export class OrderModule { }
