import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BreadcrumbService } from 'xng-breadcrumb';
import { ProductService } from 'src/app/product/product.service';

@Component({
  selector: 'app-tablet-category',
  templateUrl: './tablet-category.component.html',
  styleUrls: ['./tablet-category.component.css']
})
export class TabletCategoryComponent {
  allItems: any;
  titleCategory: any;
  data: any;
  constructor(private productService: ProductService, private route: ActivatedRoute, private bcService: BreadcrumbService) {

  }
  ngOnInit() {
    this.route.params.subscribe(params => {
      this.getProductsFromApi()
    });
  }


  getProductsFromApi(): void {
    this.productService.getProductByCategoryId('tablet').subscribe(
      (data) => {
        this.allItems = data

        this.bcService.set('@tablet', 'TABLET');
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

      this.allItems = this.allItems.filter((product: any) => product.productBrand === id);
      this.data = this.allItems
      this.titleCategory = id === 'apple' ? 'IPAD' : 'Tablet ' + id;
      this.bcService.set('@tablet', 'Tablet');
      this.bcService.set('@filterType', id);

    } else {
      console.log('error')
    }
  }

  filterPrice(index: number) {
    this.allItems = this.data.filter((product: any) => product.productSell ? product.productSell < (index * 1000000) : product.productPrice < (index * 1000000))
    this.titleCategory = 'Tablet dưới ' + index + ' triệu'
    this.bcService.set('@tablet', 'Tablet');
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
