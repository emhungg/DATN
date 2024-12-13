import { Component } from '@angular/core';
import { ProductService } from 'src/app/product/product.service';

@Component({
  selector: 'app-apple-home',
  templateUrl: './apple-home.component.html',
  styleUrls: ['./apple-home.component.css']
})
export class AppleHomeComponent {
  /**
   *
   */
  constructor(private productService: ProductService) {
  }

  ngOnInit(): void {
    //Called after the constructor, initializing input properties, and the first call to ngOnChanges.
    //Add 'implements OnInit' to the class.
    this.getAirPods();
    this.getAppleWacth();
    this.getIpad();
    this.getIphone();
    this.getMac();
  }

  items: any;

  iphoneList: any;
  ipadList: any;
  macList: any;
  appleWatchList: any;
  airPods: any;
  applePlayTech: any;

  getIphone() {
    this.productService.getProductByCategoryId('dien-thoai').subscribe(res => {
      this.iphoneList = res.filter((x: any) => x.productBrand === 'apple');
    });
  }

  getIpad() {
    this.productService.getProductByCategoryId('tablet').subscribe(res => {
      this.ipadList = res.filter((x: any) => x.productBrand === 'apple');
    });
  }
  getMac() {
    this.productService.getProductByCategoryId('lap-top').subscribe(res => {
      this.macList = res.filter((x: any) => x.productBrand === 'apple');
    });
  }
  getAppleWacth() {
    this.productService.getProductByCategoryId('dong-ho').subscribe(res => {
      this.appleWatchList = res.filter((x: any) => x.productBrand === 'apple');
    });
  }

  getAirPods() {
    this.productService.getProductByCategoryId('am-thanh').subscribe(res => {
      this.airPods = res.filter((x: any) => x.productBrand === 'apple');
    });
  }

}
