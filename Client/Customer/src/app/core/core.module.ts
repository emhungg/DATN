import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CoreRoutingModule } from './core-routing.module';
import { NotfoundComponent } from './notfound/notfound.component';
import { MaterialModule } from '../material.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { HeaderComponent } from './header/header.component';
import { FooterComponent } from './footer/footer.component';
import { BreadcrumdComponent } from './breadcrumd/breadcrumd.component';
import { BreadcrumbModule } from 'xng-breadcrumb';
import { ProductListComponent } from './product-list/product-list.component';
import { ToastrModule } from 'ngx-toastr';


@NgModule({
  declarations: [
    NotfoundComponent,
    HeaderComponent,
    FooterComponent,
    BreadcrumdComponent,
    ProductListComponent
  ],
  imports: [
    CommonModule,
    CoreRoutingModule,
    MaterialModule,
    ReactiveFormsModule,
    HttpClientModule,
    BreadcrumbModule,
    FormsModule,
    ToastrModule.forRoot()
  ],
  exports: [
    HeaderComponent,
    FooterComponent,
    BreadcrumdComponent,
    ProductListComponent
  ]
})
export class CoreModule { }
