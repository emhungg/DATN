import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { TmemberRoutingModule } from './tmember-routing.module';
import { HomeMemberComponent } from './home-member/home-member.component';
import { MaterialModule } from '../material.module';
import { DefaultComponent } from './default/default.component';
import { HistoryOderComponent } from './history-oder/history-oder.component';
import { AcountMemberComponent } from './acount-member/acount-member.component';
import { SupportComponent } from './support/support.component';
import { FeedbackComponent } from './feedback/feedback.component';
import { NgxSpinnerModule } from 'ngx-spinner';
import { ToastrModule } from 'ngx-toastr';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgxExtendedPdfViewerModule } from 'ngx-extended-pdf-viewer';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';


@NgModule({
  declarations: [
    HomeMemberComponent,
    DefaultComponent,
    HistoryOderComponent,
    AcountMemberComponent,
    SupportComponent,
    FeedbackComponent
  ],
  imports: [
    CommonModule,
    TmemberRoutingModule,
    MaterialModule,
    NgxSpinnerModule,
    ReactiveFormsModule,
    FormsModule,
    NgbModule,
    NgxExtendedPdfViewerModule,
    ToastrModule.forRoot()
  ]
})
export class TmemberModule { }
