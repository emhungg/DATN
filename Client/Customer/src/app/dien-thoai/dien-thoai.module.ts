import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { DienThoaiRoutingModule } from './dien-thoai-routing.module';
import { DienThoaiHomeComponent } from './dien-thoai-home/dien-thoai-home.component';
import { DienThoaiCateroyIdComponent } from './dien-thoai-cateroy-id/dien-thoai-cateroy-id.component';
import { SharedModule } from '../shared/shared.module';
import { CoreModule } from "../core/core.module";


@NgModule({
    declarations: [
        DienThoaiHomeComponent,
        DienThoaiCateroyIdComponent
    ],
    imports: [
        CommonModule,
        DienThoaiRoutingModule,
        SharedModule,
        CoreModule
    ]
})
export class DienThoaiModule { }
