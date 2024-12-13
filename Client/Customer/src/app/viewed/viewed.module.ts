import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ViewedRoutingModule } from './viewed-routing.module';
import { ViewedProductComponent } from './viewed-product/viewed-product.component';
import { CoreModule } from '../core/core.module';


@NgModule({
  declarations: [
    ViewedProductComponent
  ],
  imports: [
    CommonModule,
    ViewedRoutingModule,
    CoreModule
  ]
})
export class ViewedModule { }
