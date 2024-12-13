import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { TabletRoutingModule } from './tablet-routing.module';
import { TabletListComponent } from './tablet-list/tablet-list.component';
import { TabletCategoryComponent } from './tablet-category/tablet-category.component';
import { CoreModule } from "../core/core.module";
import { SharedModule } from "../shared/shared.module";


@NgModule({
    declarations: [
        TabletListComponent,
        TabletCategoryComponent
    ],
    imports: [
        CommonModule,
        TabletRoutingModule,
        CoreModule,
        SharedModule
    ]
})
export class TabletModule { }
