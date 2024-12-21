import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ListAllNewsComponent } from './list-all-news/list-all-news.component';
import { NewsDetailsComponent } from './news-details/news-details.component';

const routes: Routes = [
  {
    path: '', component: ListAllNewsComponent, data: { breadcrumb: { alisas: 'skip', skip: true } }
  },
  {
    path: ':id', component: NewsDetailsComponent, data: { breadcrumb: { alisas: 'skip', skip: true } }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class NewsRoutingModule { }
