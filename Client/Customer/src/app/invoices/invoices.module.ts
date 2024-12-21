import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from '../material.module';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { NgxSpinnerModule } from 'ngx-spinner';
import { InvoicesBillComponent } from './invoices-bill/invoices-bill.component';
import { InvoiceRoutingModule } from './invoices-routing.module';


@NgModule({
  declarations: [
    InvoicesBillComponent
  ],
  imports: [
    CommonModule,
    MaterialModule,
    ReactiveFormsModule,
    HttpClientModule,
    NgxSpinnerModule,
    InvoiceRoutingModule
  ]
})
export class InvoicesModule { }
