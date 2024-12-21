import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { LaptopRoutingModule } from './laptop-routing.module';
import { LaptopListComponent } from './laptop-list/laptop-list.component';
import { LaptopCategoryComponent } from './laptop-category/laptop-category.component';
import { CoreModule } from "../core/core.module";
import { SharedModule } from "../shared/shared.module";


@NgModule({
    declarations: [
        LaptopListComponent,
        LaptopCategoryComponent
    ],
    imports: [
        CommonModule,
        LaptopRoutingModule,
        CoreModule,
        SharedModule
    ]
})
export class LaptopModule { }
