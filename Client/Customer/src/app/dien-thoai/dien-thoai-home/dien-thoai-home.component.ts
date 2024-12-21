
import { Component } from '@angular/core';
import { ProductService } from 'src/app/product/product.service';
import { BreadcrumbService } from 'xng-breadcrumb';

@Component({
  selector: 'app-dien-thoai-home',
  templateUrl: './dien-thoai-home.component.html',
  styleUrls: ['./dien-thoai-home.component.css']
})
export class DienThoaiHomeComponent {
  allItems: any;
  categoryId: any;
  titleCategory = 'Điện thoại';
  data: any;
  constructor(private bcService: BreadcrumbService, private productService: ProductService
  ) {


  }
  ngOnInit() {
    this.getProductsFromApi();
  }
  getProductsFromApi(): void {
    this.productService.getProductByCategoryId('dien-thoai').subscribe(
      (data) => {
        this.allItems = data;
        this.data = data;
        this.bcService.set('@dienThoai', 'Điện thoại');
      },
      (error) => {
        console.error('Error fetching products', error);
      }
    );
  }


  filterPrice(index: number) {
    this.allItems = this.data.filter((product: any) => product.productSell ? product.productSell < (index * 1000000) : product.productPrice < (index * 1000000))
    this.titleCategory = 'Điện thoại dưới ' + index + ' triệu'
    this.bcService.set('@dienThoai', 'Điện thoại');
    this.bcService.set('@filterType', this.titleCategory);
  }

  sortData(order: number) {
    const customSort = (a: any, b: any): number => {
      let aValue = a.productSell === 0 ? a.productPrice : a.productSell;
      let bValue = b.productSell === 0 ? b.productPrice : b.productSell;

      if (order === 0) {
        return aValue - bValue;
      } else if (order === 1) {
        return bValue - aValue;
      } else {
        return 0;
      }
    };
    this.allItems.sort(customSort);
  }
}
