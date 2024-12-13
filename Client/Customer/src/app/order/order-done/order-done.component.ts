import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { OrderService } from '../order.service';

@Component({
  selector: 'app-order-done',
  templateUrl: './order-done.component.html',
  styleUrls: ['./order-done.component.css']
})
export class OrderDoneComponent {
  queryParams: any = {};
  currentDateTime: any;
  userInfor: any;
  listOrderItem!: any[];
  totalQuantity: number = 0;
  totalPriceWords: string = '';
  constructor(private route: ActivatedRoute, private orderService: OrderService) {
    this.currentDateTime = Date.now();
    const getUser = sessionStorage.getItem('userOrderInfor');
    const getList = sessionStorage.getItem('listItemOrder');
    this.userInfor = getUser ? JSON.parse(getUser) : [];
    this.listOrderItem = getList ? JSON.parse(getList) : [];
    sessionStorage.removeItem('userOrderInfor');
    sessionStorage.removeItem('listItemOrder');
  }

  ngOnInit() {
    this.comfirmPayment()
    this.calculateTotalQuantity()
  }

  calculateTotalQuantity() {
    for (let index = 0; index < this.listOrderItem.length; index++) {
      this.totalQuantity += this.listOrderItem[index].quantity;
    }
    this.orderService.convertPriceToNumver(this.userInfor.totalAmount).subscribe(res => {
      this.totalPriceWords = res
    })
  }

  comfirmPayment() {
    // Lấy giá trị snapshot của query parameters
    const snapshotParams = this.route.snapshot.queryParams;

    // Kiểm tra xem có query parameters hay không
    if (Object.keys(snapshotParams).length > 0) {
      // Nếu có, gán giá trị vào đối tượng queryParams
      this.queryParams = snapshotParams;

      // Tạo một đối tượng để map thông tin
      const orderInfo = {
        vnp_Amount: snapshotParams['vnp_Amount'],
        vnp_BankCode: snapshotParams['vnp_BankCode'],
        vnp_BankTranNo: snapshotParams['vnp_BankTranNo'],
        vnp_CardType: snapshotParams['vnp_CardType'],
        vnp_OrderInfo: snapshotParams['vnp_OrderInfo'],
        vnp_PayDate: snapshotParams['vnp_PayDate'],
        vnp_ResponseCode: snapshotParams['vnp_ResponseCode'],
        vnp_TmnCode: snapshotParams['vnp_TmnCode'],
        vnp_TransactionNo: snapshotParams['vnp_TransactionNo'],
        vnp_TransactionStatus: snapshotParams['vnp_TransactionStatus'],
        vnp_TxnRef: snapshotParams['vnp_TxnRef'],
        vnp_SecureHash: snapshotParams['vnp_SecureHash']
      };

      // Bạn có thể gửi orderInfo tới server ở đây
      this.sendToServer(orderInfo);
    } else {
      // Xử lý khi không có query parameters
      console.log('Không có query parameters.');
    }
  }

  sendToServer(orderInfo: any) {
    this.orderService.verifyOrder(orderInfo).subscribe(res => {
      console.log(res)
    })
  }
}
