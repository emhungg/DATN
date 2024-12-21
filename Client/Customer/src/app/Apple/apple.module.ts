import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AppleRoutingModule } from './apple-routing.module';
import { AppleHomeComponent } from './apple-home/apple-home.component';
import { SharedModule } from "../shared/shared.module";
import { CoreModule } from '../core/core.module';


@NgModule({
    declarations: [
        AppleHomeComponent
    ],
    imports: [
        CommonModule,
        AppleRoutingModule,
        SharedModule,
        CoreModule
    ]
})
export class AppleModule { }
