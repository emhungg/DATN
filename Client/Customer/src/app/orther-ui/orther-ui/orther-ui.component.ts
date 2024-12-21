import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { ProductService } from 'src/app/product/product.service';

@Component({
  selector: 'app-orther-ui',
  templateUrl: './orther-ui.component.html',
  styleUrls: ['./orther-ui.component.css']
})
export class OrtherUiComponent {
  title = '';
  constructor(private productService: ProductService, private route: ActivatedRoute) {

  }
  dataViewed: Observable<any> | undefined;

  ngOnInit(): void {
    //Called after the constructor, initializing input properties, and the first call to ngOnChanges.
    //Add 'implements OnInit' to the class.
    this.route.paramMap.subscribe(params => {
      this.getProductOther();
    })
  }
  getProductOther() {
    const id = this.route.snapshot.paramMap.get('id') as string;
    this.title = id;
    if (id === 'kho-san-pham-cu') {
      this.productService.getProduct().subscribe(res => {
        this.dataViewed = res.filter((item: any) => item.productStatus === 1);
      })
    }
    this.productService.getProductByCategoryId(id).subscribe(res => {
      this.dataViewed = res;
    });
  }
}
