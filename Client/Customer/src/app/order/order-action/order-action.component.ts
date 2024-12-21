import { Component } from '@angular/core';
import { OrderService } from '../order.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/service/auth.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { LocationService } from 'src/app/service/location.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-order-action',
  templateUrl: './order-action.component.html',
  styleUrls: ['./order-action.component.css']
})
export class OrderActionComponent {
  checkedList: any[] = [];
  totalPrice: number = 0;
  totalQuantity: number = 0;
  totalPriceWords: string = '';
  recipt: boolean = false;
  formOrder!: FormGroup;
  resProvince: any;
  provinces: any;
  constructor(private orderService: OrderService, private formBuilder: FormBuilder, private router: Router, private authService: AuthService, private spinner: NgxSpinnerService, private location: LocationService, private toastr: ToastrService) {
    const storedData = sessionStorage.getItem('listItemOrder');
    this.checkedList = storedData ? JSON.parse(storedData) : [];
  }




  ngOnInit(): void {
    //Called after the constructor, initializing input properties, and the first call to ngOnChanges.
    //Add 'implements OnInit' to the class.
    this.spinner.hide()
    this.calculateTotalPrice()
    this.formOrder = this.formBuilder.group({
      userName: this.formBuilder.control('', Validators.required),
      phoneNumber: this.formBuilder.control('', Validators.compose([Validators.pattern("^[0-9]*$"), Validators.minLength(10), Validators.maxLength(10), Validators.required])),
      email: this.formBuilder.control('', Validators.compose([Validators.email, Validators.required])),
      receiveType: '1',
      province: this.formBuilder.control('', Validators.required),
      addressReceive: this.formBuilder.control('', Validators.required),
      note: '',
      district: '',
      methodPayment: '0',
      orderItemInsertData: [],
      totalAmount: this.totalPrice,
    })
    this.location.getProvinces().subscribe(res => {
      this.resProvince = res;
      this.provinces = this.resProvince.response.data.provinces;
    });
  }
  calculateTotalPrice() {
    this.totalPrice = 0;
    this.totalQuantity = 0
    this.totalPriceWords = '';
    for (let index = 0; index < this.checkedList.length; index++) {
      const price = this.checkedList[index].priceSell > 0 ? this.checkedList[index].priceSell : this.checkedList[index].price;
      this.totalPrice += this.checkedList[index].quantity * price;
      this.totalQuantity += this.checkedList[index].quantity;
    }
    this.orderService.convertPriceToNumver(this.totalPrice).subscribe(res => {
      this.totalPriceWords = res
    })
  }

  reciptAddess(item: any) {
    this.recipt = !this.recipt;
  }


  processedOrder() {
    if (this.formOrder.valid) {
      const orderData = { ...this.formOrder.value };
      orderData.orderItemInsertData = this.checkedList.map(item => {
        return {
          productId: item.productId,
          quantity: item.quantity,
          price: item.priceSell > 0 ? item.priceSell : item.price
        };
      });

      sessionStorage.setItem('userOrderInfor', JSON.stringify(orderData));

      if (orderData.methodPayment === '0') {
        this.processRemoveItemFromLocal();
        this.processOrderPaymentAfter(orderData)
      } else {
        this.processRemoveItemFromLocal();
        this.processOrderVnPay(orderData);
      }
    } else {
      this.toastr.warning('Dữ liệu không đúng yêu cầu, vui lòng kiểm tra lại')
    }

  }
  cartItem: any;
  processRemoveItemFromLocal() {
    if (!this.authService.isUserLoggedIn()) {
      this.cartItem = localStorage.getItem('cart');
      const storedList = JSON.parse(this.cartItem);
      const remainingItems = storedList.filter((item: any) => !this.itemInList(item, this.checkedList));
      localStorage.setItem('cart', JSON.stringify(remainingItems));
    }
  }

  itemInList(item: any, list: any[]): boolean {
    return list.some((itemToRemove: any) => itemToRemove.productId === item.productId);
  }

  processOrderVnPay(item: any) {
    this.spinner.show();
    this.orderService.processCreatedOrder(item).subscribe(res => {
      window.location.href = res
    })

  }

  processOrderPaymentAfter(item: any) {
    this.spinner.show();
    this.orderService.processCreatedOrder(item).subscribe(res => {
      this.router.navigate(['/order/return'])
    })
  }

  resDistricts: any;
  districts: any;
  onProvinceChange(event: any) {
    this.location.getDistricts(event.target.value).subscribe(res => {
      this.resDistricts = res;
      this.districts = this.resDistricts.response.data.districts;
    });

    const selectedIndex = event.target.selectedIndex;

    // Lấy giá trị (label) của option được chọn
    const selectedLabel = event.target.options[selectedIndex].text;
    this.formOrder.get('province')?.setValue(selectedLabel)
  }

  onDistrictChange(event: any) {
    const selectedIndex = event.target.selectedIndex;

    // Lấy giá trị (label) của option được chọn
    const selectedLabel = event.target.options[selectedIndex].text;
    this.formOrder.get('district')?.setValue(selectedLabel)
  }
}
