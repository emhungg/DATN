import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { OrtherUIRoutingModule } from './orther-ui-routing.module';
import { OrtherUiComponent } from './orther-ui/orther-ui.component';
import { CoreModule } from '../core/core.module';


@NgModule({
  declarations: [
    OrtherUiComponent
  ],
  imports: [
    CommonModule,
    OrtherUIRoutingModule,
    CoreModule
  ]
})
export class OrtherUIModule { }
