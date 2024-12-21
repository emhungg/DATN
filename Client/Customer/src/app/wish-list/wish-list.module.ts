import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { WishListRoutingModule } from './wish-list-routing.module';
import { WishListDestailsComponent } from './wish-list-destails/wish-list-destails.component';
import { CoreModule } from '../core/core.module';


@NgModule({
  declarations: [
    WishListDestailsComponent
  ],
  imports: [
    CommonModule,
    WishListRoutingModule,
    CoreModule
  ]
})
export class WishListModule { }
