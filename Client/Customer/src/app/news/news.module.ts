import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { NewsRoutingModule } from './news-routing.module';
import { ListAllNewsComponent } from './list-all-news/list-all-news.component';
import { NewsDetailsComponent } from './news-details/news-details.component';


@NgModule({
  declarations: [
    ListAllNewsComponent,
    NewsDetailsComponent 
  ],
  imports: [
    CommonModule,
    NewsRoutingModule
  ]
})
export class NewsModule { }
