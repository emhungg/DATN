import { BreadcrumbService } from 'xng-breadcrumb';
import { Component } from '@angular/core';
import { ProductService } from '../product.service';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from 'src/app/service/auth.service';
import { ToastrService } from 'ngx-toastr';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import { CartService } from 'src/app/cart/cart.service';
import { cartItem } from 'src/app/cart/cart-type';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.css']
})
export class ProductDetailsComponent {
  // convert to html
  htmlContent!: SafeHtml;
  showFullContent = false;
  dataProductDetail: any;
  ItemSimilar: any;
  cateID: any;
  formRating!: FormGroup;
  // rating total from db
  // rating by user 
  constructor(private productService: ProductService, private route: ActivatedRoute, private brcrumb: BreadcrumbService, private authService: AuthService, private noti: ToastrService, private sanitizer: DomSanitizer, private cartService: CartService, private formBulider: FormBuilder) {

  }
  ngOnInit(): void {
    //Called after the constructor, initializing input properties, and the first call to ngOnChanges.
    //Add 'implements OnInit' to the class.
    this.route.paramMap.subscribe(params => {
      this.getProductDetail();
      this.getRatingByProduct()
    });

    // rating
    for (let index = 0; index < this.starCount; index++) {
      this.ratingArr.push(index);
    }

    this.formRating = this.formBulider.group({
      productId: '',
      userName: this.formBulider.control('', Validators.required),
      comment: this.formBulider.control('', [Validators.required, Validators.minLength(15)]),
      email: this.formBulider.control('', Validators.compose([Validators.email, Validators.required])),
      phoneNumber: this.formBulider.control('', Validators.compose([Validators.pattern("^[0-9]*$"), Validators.minLength(10), Validators.maxLength(10), Validators.required])),
      rating: 0
    });


  }



  getSimilarData() {
    this.productService.getProductByCategoryId(this.cateID).subscribe((result: any) => {
      this.ItemSimilar = result.slice(0, 10)
    });

  }

  getProductDetail() {
    const id = this.route.snapshot.paramMap.get('id') as string;
    this.productService.getProductDetail(id).subscribe(res => {
      this.dataProductDetail = res;
      this.cateID = res.categoryId;
      this.brcrumb.set('@productDetail', res.productName);
      this.brcrumb.set('@detail', res.productCategory);
      // lưu sản phẩm đã xem vào localstorage
      const viewedProductsString = localStorage.getItem('viewedProducts');
      let viewedProducts = viewedProductsString ? JSON.parse(viewedProductsString) : [];
      // Check if productId already exists in viewedProducts
      const existingProductIndex = viewedProducts.findIndex((productId: any) => productId.productId === res.productId);

      // If not exists, add it to the array
      if (existingProductIndex === -1) {
        viewedProducts.push({ productId: res.productId });

        // Save the updated array to Local Storage
        localStorage.setItem('viewedProducts', JSON.stringify(viewedProducts));
      }
      // convert des to html
      const rawHtml = res.productDescription;

      // Sanitize the HTML content to prevent security issues
      this.htmlContent = this.sanitizer.bypassSecurityTrustHtml(rawHtml);
      this.getSimilarData();
    }, error => {
      console.log(error)
    });
  }

  toggleHeart(item: any) {
    if (this.authService.isUserLoggedIn()) {
      item.isLike = !item.isLike;
      this.productService.ChangeWishPoduct(item.productId).subscribe(res => {
        this.noti.success(res + ' ' + item.productName + ' vào danh sách yêu thích')
      }, error => {
        this.noti.error('Có lỗi xin thử lại')

      })
    }
    else {
      this.noti.info('Bạn cần đăng nhập để sử dụng tính năng này.');
    }
  }
  // show content
  toggleContentVisibility() {
    this.showFullContent = !this.showFullContent;
  }


  // add to cart
  addToCart(): void {

    if (this.authService.isUserLoggedIn()) {
      const cartInsertData = {
        cartInsertData: [
          {
            productId: this.dataProductDetail.productId || '',
            quantity: 1,
            price: this.dataProductDetail.productSell > 0 ? this.dataProductDetail.productSell : this.dataProductDetail.productPrice
          }]
      }
      this.cartService.syncCartWithDatabase(cartInsertData).subscribe(res => {
        this.noti.success('Thêm sản phẩm vào giỏ hàng thành công!', "", {
          positionClass: 'toast-bottom-center'
        })
        this.cartService.getCartItems().subscribe((cartItems) => {
        });
        this.cartService.notifyCartChanged()
      }, error => {
        this.noti.error('Thêm sản phẩm vào giỏ hàng không thành công!')
      })
      this.cartService.clearCart();
    } else {
      debugger
      const totalQuantity = this.dataProductDetail.productQuantity;
      console.log(totalQuantity)
      const newItem: cartItem = {
        productId: this.dataProductDetail.productId,
        productName: this.dataProductDetail.productName,
        priceSell: this.dataProductDetail.productSell,
        price: this.dataProductDetail.productPrice,
        imagePath: this.dataProductDetail.images[0].imagePath,
        totalQuantity: totalQuantity,
        isSelected: false,
        quantity: 1,
      };
      this.cartService.getCartItems().subscribe((cartItems: any[]) => {
        const existingItem = cartItems.find((cartItem) => cartItem.productId === this.dataProductDetail.productId);
        if (existingItem) {

          if (existingItem.quantity < totalQuantity) {
            this.cartService.addToCart(existingItem);
            this.noti.success('Thêm sản phẩm vào giỏ hàng thành công!', "", {
              positionClass: 'toast-top-left'
            })
          } else {
            this.noti.error('Số lượng sản phẩm đã vượt quá giới hạn.');
          }
        } else {

          if (0 < totalQuantity) {
            console.log(newItem)
            this.cartService.addToCart(newItem);
            this.noti.success('Thêm sản phẩm vào giỏ hàng thành công!', "", {
              positionClass: 'toast-top-left'
            })
          } else {
            this.noti.error('Số lượng sản phẩm đã vượt quá giới hạn.');
          }
        }
      });
    }

  }


  //#region  handle rating 
  rating: number = 0;
  starCount: number = 5;
  color: string = 'warn';

  ratingArr: number[] = [];

  onClick(rating: number) {
    this.rating = rating;
  }

  // show rating form db
  showTnotalRating(ratingTotal: number, index: number) {
    if (ratingTotal >= index + 1) {
      return 'star';
    } else {
      return 'star_border';
    }
  }


  showRatingOfUser(ratingTotal: number, index: number) {
    if (ratingTotal >= index + 1) {
      return 'star';
    } else {
      return 'star_border';
    }
  }

  // show when user click
  showIcon(index: number) {
    if (this.rating >= index + 1) {
      return 'star';
    } else {
      return 'star_border';
    }
  }


  get ratingVote() {
    return this.formRating.get('rating');
  }

  get comment() {
    return this.formRating.get('comment');
  }

  get productId() {
    return this.formRating.get('productId');
  }

  get phoneNumber() {
    return this.formRating.get('phoneNumber');
  }

  sendProcessRating() {
    if (this.authService.isUserLoggedIn()) {
      this.ratingVote?.setValue(this.rating);
      this.productId?.setValue(this.dataProductDetail.productId);
      if (this.formRating.valid) {
        this.productService.processRating(this.formRating.value).subscribe(res => {
          this.noti.success('Gửi đánh giá thành công', 'Đã đánh giá')
          this.getRatingByProduct();
        }, error => {
          this.noti.error('Có lỗi xảy ra xin thử lại', 'Lỗi')
        })

        this.formRating.reset();
      }
      else {
        this.noti.error('Nhập đầy đủ các nội dung được yêu cầu', 'Lỗi')
        if (this.comment?.errors && this.comment.errors['minlength']) {
          this.noti.error('Nội dung đánh giá phải có từ 15 ký tự', 'Lỗi')
        } else if (this.phoneNumber?.errors) {
          this.noti.error('Số điện thoại không chính xác', 'Lỗi')
        }
      }
    } else {
      this.noti.error('Bạn cần đăng nhập để có thể đánh giá sản phẩm này', 'Lỗi')
    }

  }

  dataRating: any;

  getRatingByProduct() {
    const id = this.route.snapshot.paramMap.get('id') as string;
    console.log(id)
    this.productService.getratingByProductID(id).subscribe(res => {
      this.dataRating = res;
      console.log(res)
    })
  }
  //#endregion

} 
