import { Observable } from 'rxjs';
import { Component } from '@angular/core';
import { ProductService } from 'src/app/product/product.service';

@Component({
  selector: 'app-viewed-product',
  templateUrl: './viewed-product.component.html',
  styleUrls: ['./viewed-product.component.css']
})
export class ViewedProductComponent {
  /**
   *
   */
  constructor(private productService: ProductService) {
    this.getViewedProduct();
  }
  dataViewed: Observable<any> | undefined;
  getViewedProduct() {
    const viewedProductsString = localStorage.getItem('viewedProducts');
    let viewedProducts = viewedProductsString ? JSON.parse(viewedProductsString) : [];
    const productIdData = {
      productIdData: viewedProducts.map((item: any) => {
        return {
          productId: item.productId
        };
      })
    };

    this.productService.getViewedProduct(productIdData).subscribe(res => {
      this.dataViewed = res;
    });
  }
}
