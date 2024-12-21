import { Component, DoCheck, HostListener } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { auto } from '@popperjs/core';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from 'src/app/account/account.service';
import { LoginComponent } from 'src/app/account/login/login.component';
import { RegisterComponent } from 'src/app/account/register/register.component';
import { CartService } from 'src/app/cart/cart.service';
import { ProductService } from 'src/app/product/product.service';
import { HeaderService } from 'src/app/service/header.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements DoCheck {
  isLoggedIn: boolean = false;
  searchForm!: any;
  cartItemCount: number = 0;
  constructor(private router: Router, public service: AccountService, private toastr: ToastrService, public headerService: HeaderService, private formBuilder: FormBuilder, private dialog: MatDialog, private cartService: CartService) { }

  // form search
  ngOnInit() {
    this.searchForm = this.formBuilder.group({
      searchTerm: ['']
    });
    // this.cartService.getCartItems().subscribe(res => {
    // })
    this.cartService.cartItems$.subscribe((cartItems) => {
      // Xử lý sự thay đổi trong giỏ hàng ở đây
      this.cartItemCount = this.cartService.getTotalQuantity();
    });
  }
  searchProcess() {
    const searchTerm = this.searchForm.get('searchTerm').value;
    if (searchTerm.trim() !== '') {
      this.router.navigate(['/tim-kiem'], { queryParams: { id: searchTerm } });
      this.searchForm.get('searchTerm').setValue('');
    } else {
      this.toastr.error('Bạn cần nhập từ khóa đế tìm kiếm')
    }
  }

  //#region style for nav
  isHeaderFixed: boolean = false;

  @HostListener('window:scroll', [])
  onScroll(): void {
    const scrollPosition = window.pageYOffset || document.documentElement.scrollTop || document.body.scrollTop || 0;
    this.isHeaderFixed = scrollPosition > 500; // Thay 100 bằng ngưỡng bạn muốn cố định thanh điều hướng
  }
  fixedStyles = {
    'z-index': '1000',
    'position': 'fixed',
    'top': '0px',
    'color': 'blue',
    'margin-left': '0px',
    'width': '100%',
    'left': '0px',
    'box-shadow': '0'
  };

  defaultStyles = {
    'z-index': 'auto',
    'position': 'static',
    'top': 'auto'
  };
  //#endregion
  login() {
    // this.router.navigate(['/account/login']);
    const dialogConfig = new MatDialogConfig();
    const dialogRef = this.dialog.open(LoginComponent, dialogConfig);
  }
  //#region register
  register() {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.width = '1200px';
    dialogConfig.autoFocus = false;
    dialogConfig.height = '750px';
    const dialogRef = this.dialog.open(RegisterComponent, dialogConfig);
  }
  //#endregion
  doLogout() {
    Swal.fire({
      title: "Bạn muốn đăng xuất?",
      icon: 'question',
      showDenyButton: true,
      confirmButtonText: 'Đồng ý',
      denyButtonText: 'Không'
    }).then((rs) => {
      if (rs.isConfirmed) {
        try {
          this.service.logout().subscribe(res => {
            this.isLoggedIn = false;
            sessionStorage.clear();
            location.reload()
          }, error => {
            console.log(error);
            this.toastr.error("Must validate code before logout");
          });
        } catch (error) {
          console.log(error);
          this.toastr.warning("Error");
        }
      }
    })

  }
  isNavbarHidden: boolean = false;
  ngDoCheck(): void {
    //Called every time that the input properties of a component or a directive are checked. Use it to extend change detection by performing a custom check.
    //Add 'implements DoCheck' to the class.
    const adminHidden = sessionStorage.getItem('roles');
    if (adminHidden === 'ADMIN') {
      this.isNavbarHidden = false;
    } else {
      this.isNavbarHidden = true;
    }
  }


  // toggel greater than 100 mil
  navigateToList100() {
    // Chuyển hướng đến component danh sách sản phẩm
    this.router.navigate(['/dien-thoai/filter-price-13']);
  }


  keyword = 'name';
  public countries = [
    {
      id: 1,
      name: 'Albania',
    },
    {
      id: 2,
      name: 'Belgium',
    },
    {
      id: 3,
      name: 'Denmark',
    },
    {
      id: 4,
      name: 'Montenegro',
    },
    {
      id: 5,
      name: 'Turkey',
    },
    {
      id: 6,
      name: 'Ukraine',
    },
    {
      id: 7,
      name: 'Macedonia',
    },
    {
      id: 8,
      name: 'Slovenia',
    },
    {
      id: 9,
      name: 'Georgia',
    },
    {
      id: 10,
      name: 'India',
    },
    {
      id: 11,
      name: 'Russia',
    },
    {
      id: 12,
      name: 'Switzerland',
    }
  ];
  selectEvent(item:any) {
    // do something with selected item
  }

  onChangeSearch(search: string) {
    // fetch remote data from here
    // And reassign the 'data' which is binded to 'data' property.
  }

  onFocused(e:any) {
    // do something
  }

}
