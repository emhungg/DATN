import { Subscription } from 'rxjs';
import { Component } from '@angular/core';
import { SharedService } from '../shared.service';

@Component({
  selector: 'app-slide-supplier',
  templateUrl: './slide-supplier.component.html',
  styleUrls: ['./slide-supplier.component.css']
})
export class SlideSupplierComponent {
  /**
   *
   */
  constructor(private service: SharedService) {
    this.imgSubscription = this.service.getImgsSupplier().subscribe(imgs => {
      this.images = imgs
    });
  }
  images: any;
  private imgSubscription!: Subscription;


  slideConfig = {
    slidesToShow: 8,
    slidesToScroll: 1,
    autoplay: true,
    autoplaySpeed: 1500,
    speed: 1500,
    infinite: true,
    lazyLoad: "ondemand",
    pauseOnHover: true,
  };


  ngOnDestroy(): void {
    if (this.imgSubscription) {
      console.log("destroy");
      this.imgSubscription.unsubscribe();
    }
  }
}
