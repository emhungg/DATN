import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { InvoicesBillComponent } from './invoices-bill/invoices-bill.component';

const routes: Routes = [
  { path: '', component: InvoicesBillComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class InvoiceRoutingModule { }
