import { Component } from '@angular/core';
import { forkJoin } from 'rxjs';
import { ProductService } from 'src/app/product/product.service';
import { HomeService } from '../home.service';
@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})

export class HomeComponent {
  //#region selected flash sell
  selectedButton: string = 'SellTop1';

  showContent(button: string): void {
    this.selectedButton = button;
  }
  data: any[] = [];
  flashSellDL: any;
  flashSelllaptop: any;
  flashSellOrther: any;
  appleProduct: any;
  laptopList: any;
  phonetop: any;
  //#endregion
  itemsProduct: any;
  constructor(private productService: ProductService) {
  }
  titleApple = 'APPLE AUTHORISED RESELLER';
  titleProductHot = 'ĐIỆN THOẠI NỔI BẬT';


  ngOnInit(): void {
    this.loadData();
  }

  loadData() {
    this.productService.getProduct().subscribe((result: any) => {
      this.data = result;
      this.flashSellDL = this.data.filter(x => (x.categoryId === 'dien-thoai' || x.categoryId === 'tablet') && x.productSell > 0).slice(0, 10);
      this.flashSelllaptop = this.data.filter(x => (x.categoryId === 'lap-top' || x.categoryId === 'man-hinh' || x.categoryId === 'smart-tv') && x.productSell > 0).slice(0, 10);
      this.flashSellOrther = this.data.filter(x => (x.categoryId !== 'lap-top' && x.categoryId !== 'tablet' && x.categoryId !== 'dien-thoai' && x.categoryId !== 'man-hinh' && x.categoryId !== 'smart-tv') && x.productSell > 0).slice(0, 10);

      this.appleProduct = this.data.filter(x => x.productBrand === 'apple');
      this.laptopList = this.data.filter(x => x.categoryId === 'lap-top' || x.categoryId === 'pc');
      this.phonetop = this.data.filter(x => x.categoryId === 'dien-thoai');

    });

  }

}
