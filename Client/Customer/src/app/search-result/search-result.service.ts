import { Observable, of } from 'rxjs';
import { Injectable } from '@angular/core';
import { ProductService } from '../product/product.service';

@Injectable({
  providedIn: 'root'
})
export class SearchResultService {
  result: any;
  constructor(private productService: ProductService) { }
  searchProducts(searchTerm: string) {
    this.productService.getProduct().subscribe(data => {
      return data.filter((product: any) =>
        product.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
        product.category.toLowerCase().includes(searchTerm.toLowerCase()) ||
        product.brandId.toLowerCase().includes(searchTerm.toLowerCase())
      );
    })
  }
}

