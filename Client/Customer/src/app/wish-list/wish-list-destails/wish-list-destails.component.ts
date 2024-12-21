import { Component } from '@angular/core';
import { Observable } from 'rxjs';
import { ProductService } from 'src/app/product/product.service';

@Component({
  selector: 'app-wish-list-destails',
  templateUrl: './wish-list-destails.component.html',
  styleUrls: ['./wish-list-destails.component.css']
})
export class WishListDestailsComponent {
  constructor(private productService: ProductService) {
    this.getViewedProduct();
  }
  dataViewed: Observable<any> | undefined;
  getViewedProduct() {
    this.productService.getWishList().subscribe(res => {
      this.dataViewed = res;
    });
  }
}
