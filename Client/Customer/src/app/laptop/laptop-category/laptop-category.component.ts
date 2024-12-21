import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BreadcrumbService } from 'xng-breadcrumb';
import { ProductService } from 'src/app/product/product.service';

@Component({
  selector: 'app-laptop-category',
  templateUrl: './laptop-category.component.html',
  styleUrls: ['./laptop-category.component.css']
})
export class LaptopCategoryComponent {
  allItems: any;
  titleCategory: any;
  constructor(private productService: ProductService, private route: ActivatedRoute, private bcService: BreadcrumbService) {

  }
  ngOnInit() {
    this.route.params.subscribe(params => {
     this.getProductsFromApi()
    });
  }


  getProductsFromApi(): void {
    this.productService.getProductByCategoryId('lap-top').subscribe(
      (data) => {
        this.allItems = data
        this.bcService.set('@dienThoai', 'Laptop');
        this.getProductsForCategory()
      },
      (error) => {
        console.error('Error fetching products', error);
      }
    );
  }

  getProductsForCategory() {
    if (this.allItems) {
      const id = this.route.snapshot.paramMap.get('id') as string;
      if (id === 'filter-price-13') {
        this.allItems = this.allItems.filter((product: any) => product.productSell ? product.productSell > 5000000 : product.productPrice > 5000000)
        this.titleCategory = 'Laptop trên 100 triệu'
        this.bcService.set('@dienThoai', 'Laptop');
        this.bcService.set('@filterType', "Trên 100 triệu");

      } else {
        this.allItems = this.allItems.filter((product: any) => product.productBrand === id);
        this.titleCategory = id === 'apple' ? 'Macbook' : 'Laptop ' + id;
        this.bcService.set('@dienThoai', 'Laptop');
        this.bcService.set('@filterType', id);
      }
    } else {
      console.log('error')
    }
  }
}
