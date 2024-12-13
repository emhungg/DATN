import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { NewsManageRoutingModule } from './news-manage-routing.module';
import { NewsComponent } from './news/news.component';
import { PrimeNgModule } from '../primeng.module';
import { NgxPaginationModule } from 'ngx-pagination';
import { AngularEditorModule } from '@kolkov/angular-editor';
import { ReactiveFormsModule } from '@angular/forms';


@NgModule({
  declarations: [
    NewsComponent
  ],
  imports: [
    CommonModule,
    NewsManageRoutingModule,
    PrimeNgModule,
    NgxPaginationModule,
    AngularEditorModule,
    ReactiveFormsModule
  ]
})
export class NewsManageModule { }
