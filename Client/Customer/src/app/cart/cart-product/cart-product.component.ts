import { Component } from '@angular/core';
import { CartService } from '../cart.service';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from 'src/app/service/auth.service';
import Swal from 'sweetalert2';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { LoginComponent } from 'src/app/account/login/login.component';
import { Router } from '@angular/router';

@Component({
  selector: 'app-cart-product',
  templateUrl: './cart-product.component.html',
  styleUrls: ['./cart-product.component.css']
})
export class CartProductComponent {
  /**
   *
   */
  constructor(private cartService: CartService, private toastr: ToastrService, private authService: AuthService, private dialog: MatDialog, private route: Router) {
  }

  cartItems: any;

  ngOnInit(): void {
    //Called after the constructor, initializing input properties, and the first call to ngOnChanges.
    //Add 'implements OnInit' to the class.
    this.processGetCart();
    if (!this.authService.isUserLoggedIn()) {
      this.isAllSelected();
    }
  };

  processGetCart() {
    this.cartService.getCartItems().subscribe(res => {
      this.cartItems = res;
      console.log(res)
    })
    this.cartService.cartItems$.subscribe((cartItems) => {
      this.cartItems = cartItems;
    });
  }

  // handle for check
  masterSelected: boolean = false;
  checkedList: any;
  itemSelected: boolean = false;
  totalPrice: number = 0;
  countItemCheck: number = 0;
  checkUncheckAll() {
    for (var i = 0; i < this.cartItems.length; i++) {
      this.cartItems[i].isSelected = this.masterSelected;
    }
    if (!this.authService.isUserLoggedIn()) {
      this.cartService.updateCartItems(this.cartItems);
    }

    this.getCheckedItemList();

  }
  isAllSelected() {
    this.masterSelected = this.cartItems.every(function (item: any) {
      return item.isSelected == true;
    })
    this.getCheckedItemList();
    if (!this.authService.isUserLoggedIn()) {
      this.cartService.updateCartItems(this.cartItems);
    }

  }

  getCheckedItemList() {
    this.checkedList = [];
    this.totalPrice = 0;
    this.countItemCheck = 0;
    for (var i = 0; i < this.cartItems.length; i++) {
      if (this.cartItems[i].isSelected) {
        this.checkedList.push(this.cartItems[i]);
        const price = this.cartItems[i].priceSell > 0 ? this.cartItems[i].priceSell : this.cartItems[i].price
        this.totalPrice += price * this.cartItems[i].quantity;
        this.countItemCheck += this.cartItems[i].quantity;
      }
    }

    this.itemSelected = this.checkedList.length > 0;
  }

  orderProduct() {
    this.route.navigate(['order']);
    sessionStorage.setItem('listItemOrder', JSON.stringify(this.checkedList));
  }
  // Hàm xử lý khi nút "+" được click
  increaseQuantity(index: number) {
    const maxQuantity = this.cartItems[index].totalQuantity;
    if (this.cartItems[index].quantity < maxQuantity) {
      const quantity = this.cartItems[index].quantity++;
      if (this.authService.isUserLoggedIn()) {
        const cartUpdateQuantity = {
          productId: this.cartItems[index].productId || '',
          quantity: quantity + 1,
          price: 0
        }

        this.cartService.updateQuantity(cartUpdateQuantity).subscribe(res => {
          console.log(res)
          this.cartService.notifyCartChanged()
        });
      }
      else {
        this.cartService.updateCartItems(this.cartItems);
      }

      this.calculationTotalPrice()
    } else {
      this.toastr.error('Sản phẩm đã đạt đến số lượng tối đa.');
    }

  }

  // Hàm xử lý khi nút "-" được click
  changeQuantity(index: number, event: any) {
    const maxQuantity = this.cartItems[index].totalQuantity;
    const quantityString = event.value;
    const quantity = parseInt(quantityString, 10);
    if (quantity <= 0) {
      this.toastr.error('Số lượng không thể giảm xuống dưới 1.');
    } else if (quantity > maxQuantity) {
      this.toastr.error('Sản phẩm đã đạt đến số lượng tối đa.');
    } else {
      this.cartItems[index].quantity = quantity;
      if (this.authService.isUserLoggedIn()) {
        const cartUpdateQuantity = {
          productId: this.cartItems[index].productId || '',
          quantity: quantity,
          price: 0
        }
        this.cartService.updateQuantity(cartUpdateQuantity).subscribe(res => {
          console.log(res)
          this.cartService.notifyCartChanged()
        });
      } else {
        this.cartService.updateCartItems(this.cartItems);
      }
      this.calculationTotalPrice()
    }
  }

  handleEnterKey(index: number, event: any) {
    if (event.key === 'Enter') {
      // Ngăn chặn hành động mặc định của phím Enter trên trình duyệt
      event.preventDefault();
      // Lấy giá trị từ input và gọi hàm changeQuantity
      const inputElement = event.target as HTMLInputElement;
      this.changeQuantity(index, inputElement);
    }
  }
  

  // Hàm xử lý khi nút "-" được click
  decreaseQuantity(index: number) {
    if (this.cartItems[index].quantity > 1) {
      // Giảm số lượng nếu nó lớn hơn 1
      const quantity = this.cartItems[index].quantity--;
      if (this.authService.isUserLoggedIn()) {
        const cartUpdateQuantity = {
          productId: this.cartItems[index].productId || '',
          quantity: quantity - 1,
          price: 0
        }
        this.cartService.updateQuantity(cartUpdateQuantity).subscribe(res => {
          console.log(res)
          this.cartService.notifyCartChanged()
        });
      } else {
        this.cartService.updateCartItems(this.cartItems);
      }

      this.calculationTotalPrice()
    } else {
      this.toastr.error('Số lượng không thể giảm xuống dưới 1.');
    }


  }
  removeItem(item: any) {
    const index = this.cartItems.indexOf(item);
    if (index !== -1) {
      const cartItemsData = {
        cartInsertData: [{
          productId: this.cartItems[index].productId || '',
          quantity: 0,
          price: 0
        }]
      }

      this.cartItems.splice(index, 1);
      if (this.authService.isUserLoggedIn()) {
        this.cartService.deleteItems(cartItemsData).subscribe(res => {
          this.toastr.success("Đã xóa sản phẩm khỏi giỏ hàng.")
          this.cartService.notifyCartChanged()

        }, error => {
          this.toastr.error("Có lỗi trong quá trình xóa.")
          this.cartService.notifyCartChanged()

        });
      } else {
        this.cartService.updateCartItems(this.cartItems);
      }
    }


  }



  removeItemsInCart() {
    this.checkedList = [];
    this.totalPrice = 0;
    for (let i = this.cartItems.length - 1; i >= 0; i--) {
      if (this.cartItems[i].isSelected) {
        this.checkedList.push(this.cartItems[i]);
        this.cartItems.splice(i, 1);
      }
    }
    const cartListData = {
      cartInsertData: this.checkedList.map((item: any) => {
        return {
          productId: item.productId || '',
          quantity: 0,
          price: 0
        };

      })
    }
    if (this.authService.isUserLoggedIn()) {
      this.cartService.deleteItems(cartListData).subscribe(res => {
        this.toastr.success("Đã xóa các sản phẩm được chọn khỏi giỏ hàng.")
        this.cartService.notifyCartChanged()

      }, error => {
        this.toastr.error("Có lỗi trong quá trình xóa.")
        this.processGetCart();
      });
    } else {
      this.cartService.updateCartItems(this.cartItems);
    }


  }

  calculationTotalPrice() {
    this.totalPrice = 0;
    this.countItemCheck = 0;
    for (var i = 0; i < this.cartItems.length; i++) {
      if (this.cartItems[i].isSelected) {
        const price = this.cartItems[i].priceSell > 0 ? this.cartItems[i].priceSell : this.cartItems[i].price
        this.totalPrice += price * this.cartItems[i].quantity
        this.countItemCheck += this.cartItems[i].quantity;
      }
    }
  }

}

