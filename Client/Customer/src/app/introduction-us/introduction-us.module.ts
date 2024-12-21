import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { IntroductionUsRoutingModule } from './introduction-us-routing.module';
import { IntroductionComponent } from './introduction/introduction.component';


@NgModule({
  declarations: [
    IntroductionComponent
  ],
  imports: [
    CommonModule,
    IntroductionUsRoutingModule
  ]
})
export class IntroductionUsModule { }
