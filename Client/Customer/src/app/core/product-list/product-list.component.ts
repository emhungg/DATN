import { Component, Input } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { ProductService } from 'src/app/product/product.service';
import { AuthService } from 'src/app/service/auth.service';

@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.css']
})
export class ProductListComponent {
  /**
   *
   */
  constructor(private noti: ToastrService, private serviceProduct: ProductService, private auth: AuthService) {

  }
  @Input() title: any;
  @Input() items: any;
  toggleHeart = (item: any) => {
    if (this.auth.isUserLoggedIn()) {
      item.isLike = !item.isLike;
      this.serviceProduct.ChangeWishPoduct(item.productId).subscribe(res => {
        this.noti.success(res + ' ' + item.productName + ' vào danh sách yêu thích')
      }, error => {
        this.noti.error('Có lỗi xin thử lại')

      })
    } else {
      this.noti.info('Bạn cần đăng nhập để sử dụng tính năng này');
    }

  }
  ngOnInit(): void {
    //Called after the constructor, initializing input properties, and the first call to ngOnChanges.
    //Add 'implements OnInit' to the class.
  }
  // config show more 
  displayedItems = 20; // Số lượng mục muốn hiển thị ban đầu
  remainingItems(): number {
    if (this.items && typeof this.items.length !== 'undefined' && this.items.length > this.displayedItems) {
      return this.items.length - this.displayedItems;

    } return 0;
  }
  loadMore() {
    // Khi người dùng nhấn nút "Xem thêm"
    this.displayedItems += 20; // Tăng số lượng mục muốn hiển thị
  }
}
