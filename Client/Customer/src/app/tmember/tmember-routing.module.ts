import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeMemberComponent } from './home-member/home-member.component';
import { DefaultComponent } from './default/default.component';
import { HistoryOderComponent } from './history-oder/history-oder.component';
import { AcountMemberComponent } from './acount-member/acount-member.component';
import { SupportComponent } from './support/support.component';
import { FeedbackComponent } from './feedback/feedback.component';

const routes: Routes = [
  {
    path: '', component: HomeMemberComponent, data: { breadcrumb: 'Tài khoản' },
    children: [
      {
        path: 'home', component: DefaultComponent
      },
      {
        path: 'history-order', component: HistoryOderComponent, data: { breadcrumb: 'Lịch sử đặt hàng' }
      },
      {
        path: 'account/user-info', component: AcountMemberComponent, data: { breadcrumb: 'Chi tiết' }
      },
      {
        path: 'account/support', component: SupportComponent, data: { breadcrumb: 'Hỗ trợ' }
      }, {
        path: 'account/feedback', component: FeedbackComponent, data: { breadcrumb: 'Phản hồi' }
      }
    ]
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TmemberRoutingModule { }
