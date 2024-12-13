import { Component, ElementRef, ViewChild } from '@angular/core';
import { HistoryOderService } from './history-oder.service';
import { InvoicesService } from 'src/app/invoices/invoices.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-history-oder',
  templateUrl: './history-oder.component.html',
  styleUrls: ['./history-oder.component.css']
})
export class HistoryOderComponent {
  selectedButton: string = 'isAll';
  historyOrdeInfor: any;
  isShowDetail: boolean = false;
  orderDetails: any;
  pdfurl = '';

  @ViewChild('content') popupview !: ElementRef;
  /**
   *
   */
  constructor(private service: HistoryOderService, private invoiceService: InvoicesService, private modalservice: NgbModal,private spinner: NgxSpinnerService) {

  }

  ngOnInit(): void {
    //Called after the constructor, initializing input properties, and the first call to ngOnChanges.
    //Add 'implements OnInit' to the class.
    this.getHistoryInfor();
  }

  filteredHistoryOrder: any[] = [];
  showContent(button: string): void {
    this.selectedButton = button;
    switch (button) {
      case 'isAll':
        this.filteredHistoryOrder = [...this.historyOrdeInfor.groupOrderItems];
        break;
      case 'isWattingConfirm':
        this.filteredHistoryOrder = this.historyOrdeInfor.groupOrderItems.filter((i: any) => i.statusReceiveOrder === 3);
        break;
      case 'isShipping':
        this.filteredHistoryOrder = this.historyOrdeInfor.groupOrderItems.filter((i: any) => i.statusReceiveOrder === 0);
        break;
      case 'isShipped':
        this.filteredHistoryOrder = this.historyOrdeInfor.groupOrderItems.filter((i: any) => i.statusReceiveOrder === 1);
        break;
      case 'isCanceled':
        this.filteredHistoryOrder = this.historyOrdeInfor.groupOrderItems.filter((i: any) => i.statusReceiveOrder === 2);
        break;
    }
  }

  // get data total
  getHistoryInfor() {
    this.service.getHistoryInfor().subscribe(res => {
      this.historyOrdeInfor = res;
      this.filteredHistoryOrder = [...res.groupOrderItems];
    })
  }


  getNameStatusOrderReceive(code: number) {
    switch (code) {
      case 1: return 'Đã giao hàng';
      case 0: return 'Đang vận chuyển';
      case 2: return 'Đã hủy';
      case 3: return 'Chờ xác nhận';
      default:
        return 'Chờ xác nhận';
    }
  }

  getClass(code: number) {
    switch (code) {
      case 1: return 'shipped';
      case 0: return 'shipping';
      case 2: return 'cancelled';
      case 3: return 'waitting';
      default:
        return 'Chờ xác nhận';
    }
  }

  showDetailOrder() {
    this.isShowDetail = !this.isShowDetail;
  }

  userInfor!: any;
  itemDetails: any;
  getOrderDetails(item: any) {
    this.showDetailOrder();
    this.orderDetails = item;
    this.userInfor = this.service.getUserInfor()
    this.service.getOrderDetail(item.orderId).subscribe(res => {
      this.itemDetails = res
    })
  }


  ViewInvoice(item: any) {
    this.spinner.show();
    this.invoiceService.GenerateInvoicLink(item.orderId).subscribe(res => {
      this.spinner.hide();
      window.open(res, '_blank');
    }, error => {
      console.log(error)
    })
  }


  PrintInvoice(item: any) {
    this.spinner.show();
    this.invoiceService.GenerateInvoicePDF(item.orderId).subscribe(res => {
      let blob: Blob = res.body as Blob;
      let url = window.URL.createObjectURL(blob);
      this.pdfurl = url;
      this.modalservice.open(this.popupview, { size: 'lg' });
      this.spinner.hide();
    }, error => {
      console.log(error)
    })
  }


  DownloadInvoice(item: any) {
    this.spinner.show();
    this.invoiceService.GenerateInvoicePDF(item.orderId).subscribe(res => {
      let blob: Blob = res.body as Blob;
      let url = window.URL.createObjectURL(blob);

      let a = document.createElement('a');
      a.download = item.orderId;
      a.href = url;
      a.click();
      this.spinner.hide();
    });
  }

}
