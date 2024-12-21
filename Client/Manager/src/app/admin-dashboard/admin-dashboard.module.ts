import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AdminDashboardRoutingModule } from './admin-dashboard-routing.module';
import { TrangChuComponent } from './trang-chu/trang-chu.component';
import { SideNavComponent } from './side-nav/side-nav.component';
import { BodyComponent } from './body/body.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { NavBarComponent } from './nav-bar/nav-bar.component';
import { CoreModule } from "./core/core.module";
import { NgChartsModule } from 'ng2-charts';


@NgModule({
    declarations: [
        TrangChuComponent,
        SideNavComponent,
        BodyComponent,
        DashboardComponent,
        NavBarComponent
    ],
    imports: [
        CommonModule,
        AdminDashboardRoutingModule,
        CoreModule,
        NgChartsModule
    ]
})
export class AdminDashboardModule { }
