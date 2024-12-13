import { Component, ElementRef, Input, ViewChild } from '@angular/core';
import { OwlOptions } from 'ngx-owl-carousel-o';
@Component({
  selector: 'app-carousel-product-details',
  templateUrl: './carousel-product-details.component.html',
  styleUrls: ['./carousel-product-details.component.css']
})
export class CarouselProductDetailsComponent {
  @Input() items: any;
  constructor(private elRef: ElementRef) {  }
  customThumbsOptions: OwlOptions = {
    loop: true,
    autoplay: true,
    center: false,
    dots: true,
    autoHeight: true,
    autoWidth: true,
    autoplayTimeout: 5000,
    smartSpeed: 2000,
    navSpeed: 1000,
    lazyLoad: true,
    navText: ['<i class="fal fa-angle-left"></i>', '<i class="fal fa-angle-right"></i>'],
    responsive: {
      0: {
        items: 1,
      },
      600: {
        items: 1,
      },
      1000: {
        items: 1,
      },
    },
    nav: true
  };

  

}
