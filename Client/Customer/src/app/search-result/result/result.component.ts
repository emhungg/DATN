import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { SearchResultService } from '../search-result.service';
import { BreadcrumbService } from 'xng-breadcrumb';
import { ProductService } from 'src/app/product/product.service';

@Component({
  selector: 'app-result',
  templateUrl: './result.component.html',
  styleUrls: ['./result.component.css']
})
export class ResultComponent {
  allItems: any;
  titleCategory: any;
  notFoundProduct = false;
  keyFind: any;
  /**
   *
   */
  constructor(private route: ActivatedRoute, private productService: ProductService, private brcrumbService: BreadcrumbService) {

  }

  ngOnInit(): void {
    //Called after the constructor, initializing input properties, and the first call to ngOnChanges.
    //Add 'implements OnInit' to the class.
    this.loadProduct();
  }

  loadProduct() {
    this.route.queryParams.subscribe(params => {
      const id = params['id']; //get id from params
      this.keyFind = id;
      this.titleCategory = 'Kết quả tìm kiếm: ' + this.keyFind;
      this.productService.getProduct().subscribe(data => {
        this.allItems = data.filter((product: any) =>
          product.productName.toLowerCase().includes(id.toLowerCase()) ||
          product.productCategory.toLowerCase().includes(id.toLowerCase()) ||
          product.productBrand.toLowerCase().includes(id.toLowerCase())
        );
        this.notFoundProduct = false;
        if (this.allItems.length === 0) {
          this.notFoundProduct = true;
        }
      })
      this.brcrumbService.set('@timKiem', 'Tìm kiếm');

    });
  }
}
