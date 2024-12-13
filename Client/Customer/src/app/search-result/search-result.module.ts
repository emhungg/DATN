import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SearchResultRoutingModule } from './search-result-routing.module';
import { ResultComponent } from './result/result.component';
import { CoreModule } from "../core/core.module";


@NgModule({
    declarations: [
        ResultComponent
    ],
    imports: [
        CommonModule,
        SearchResultRoutingModule,
        CoreModule
    ]
})
export class SearchResultModule { }
