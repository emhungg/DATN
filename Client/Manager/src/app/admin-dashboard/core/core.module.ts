import { NgChartsModule } from 'ng2-charts';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CoreRoutingModule } from './core-routing.module';
import { BreadcrumdComponent } from './breadcrumd/breadcrumd.component';
import { BreadcrumbModule } from 'xng-breadcrumb';
import { LineChartComponent } from './line-chart/line-chart.component';
import { PieChartComponent } from './pie-chart/pie-chart.component';


@NgModule({
  declarations: [
    BreadcrumdComponent,
    LineChartComponent,
    PieChartComponent
  ],
  imports: [
    CommonModule,
    CoreRoutingModule,
    BreadcrumbModule,
    NgChartsModule
  ],
  exports: [
    BreadcrumdComponent,
    LineChartComponent,
    PieChartComponent
  ]
})
export class CoreModule { }
