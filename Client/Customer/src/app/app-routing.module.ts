import { DienThoaiRoutingModule } from './dien-thoai/dien-thoai-routing.module';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/section-home/home.component';
import { NotfoundComponent } from './core/notfound/notfound.component';
import { authGuard } from './guard/auth.guard';
import { authIsactiveGuard } from './guard/auth-isactive.guard';

const routes: Routes = [
  {
    path: '', component: HomeComponent, data: {
      breadcrumb: {
        label: 'Trang chủ', info: 'home'
      }
    }
  },
  {
    path: 'home', component: HomeComponent, data: {
      breadcrumb: {
        label: 'Trang chủ', info: 'home'
      }
    }
  },
  // add lazy loading for account
  {
    path: 'account',
    loadChildren: () => import('./account/account.module').then(m => m.AccountModule),
    data: {
      breadcrumb: { skip: true, alias: 'mentorAdd' }
    }
  },

  // add lazy loading for member page
  {
    path: 'tmember',
    canActivate: [authGuard, authIsactiveGuard],
    loadChildren: () => import('./tmember/tmember.module').then(m => m.TmemberModule),
    data: {
      breadcrumb: { skip: true, alias: 'mentorAdd' }
    }
  },
  // add lazy load for introduction
  {
    path: 'intro',
    loadChildren: () => import('./introduction-us/introduction-us.module').then(m => m.IntroductionUsModule),
    data: {
      breadcrumb: { skip: true, alias: 'mentorAdd' }
    }
  },
  // add lazy load dien thoai
  {
    path: 'dien-thoai',
    loadChildren: () => import('./dien-thoai/dien-thoai.module').then(m => m.DienThoaiModule),
    data: {
      breadcrumb: { alias: 'dienThoai' }
    }
  },
  // add lazy load apple
  {
    path: 'apple',
    loadChildren: () => import('./Apple/apple.module').then(m => m.AppleModule),
    data: { breadcrumb: { skip: true, alias: 'skipApple' } }
  },
  // add lazy load search
  {
    path: 'tim-kiem',
    loadChildren: () => import('./search-result/search-result.module').then(m => m.SearchResultModule),
    data: { breadcrumb: { label: 'Tìm kiếm', alias: 'timKiem' } }
  },
  // add lazy loading for laptop sc
  {
    path: 'laptop',
    loadChildren: () => import('./laptop/laptop.module').then(m => m.LaptopModule),
    data: { breadcrumb: { alias: 'laptop' } }
  },
  // add lazy loading for laptop sc
  {
    path: 'tablet',
    loadChildren: () => import('./tablet/tablet.module').then(m => m.TabletModule),
    data: { breadcrumb: { alias: 'tablet' } }
  },
  {
    path: 'product-details',
    loadChildren: () => import('./product/product.module').then(m => m.ProductModule)
    , data: { breadcrumb: { alias: 'detail' } }
  },
  {
    path: 'viewed',
    loadChildren: () => import('./viewed/viewed.module').then(m => m.ViewedModule)
  },
  {
    path: 'cart',
    loadChildren: () => import('./cart/cart.module').then(m => m.CartModule)
  },
  {
    path: 'order',
    loadChildren: () => import('./order/order.module').then(m => m.OrderModule)
  },
  {
    path: 'san-pham-yeu-thich',
    loadChildren: () => import('./wish-list/wish-list.module').then(m => m.WishListModule),
    data: {
      breasdcrumb: { skip: true, alias: 'mentorAdd' }
    }
  },
  {
    path: 'tin-tuc', loadChildren: () => import('./news/news.module').then(m => m.NewsModule)
  },
  {
    path: 'other',
    loadChildren: () => import('./orther-ui/orther-ui.module').then(m => m.OrtherUIModule),
    data: {
      breasdcrumb: { alias: 'other' }
    }
  }
  ,
  { path: '**', component: NotfoundComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
