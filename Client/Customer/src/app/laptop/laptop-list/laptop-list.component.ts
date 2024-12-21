import { Component } from '@angular/core';
import { BreadcrumbService } from 'xng-breadcrumb';
import { ProductService } from 'src/app/product/product.service';

@Component({
  selector: 'app-laptop-list',
  templateUrl: './laptop-list.component.html',
  styleUrls: ['./laptop-list.component.css']
})
export class LaptopListComponent {
  allItems: any;
  categoryId: any;
  titleCategory = 'Laptop';
  constructor(private bcService: BreadcrumbService, private productService: ProductService
  ) {


  }
  ngOnInit() {
    this.getProductsFromApi();
  }
  getProductsFromApi(): void {
    this.productService.getProductByCategoryId('lap-top').subscribe(
      (data) => {
        this.allItems = data;
        this.bcService.set('@dienThoai', 'Laptop');
      },
      (error) => {
        console.error('Error fetching products', error);
      }
    );
  }


}
