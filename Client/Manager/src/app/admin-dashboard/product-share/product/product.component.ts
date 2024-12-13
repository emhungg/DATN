import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { Subscription } from 'rxjs';

import Swal from 'sweetalert2';
import { ProductUpdateComponent } from '../product-update/product-update.component';
import { ProductAddComponent } from '../product-add/product-add.component';
import { ProductService } from '../product.service';

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.scss']
})
export class ProductComponent {
  constructor(private dialog: MatDialog, private service: ProductService) {
  }
  //#region  fillter
  textFillter: any;
  allObject: any;
  filterResults() {
    if (!this.textFillter) {
      this.ngOnInit();
    }
    this.allObject = this.POSTS.filter((res: any) => {
      return res.productName.toLowerCase().includes(this.textFillter.toLowerCase())
    }
    );
  }
  //#endregion
  //#region  sort data
  sortOrder: { key: string; reverse: boolean } = { key: '', reverse: false };

  sortObjects(key: string): void {
    console.log(key)
    this.sortOrder.reverse = !this.sortOrder.reverse;
    this.sortOrder.key = key;
    this.POSTS.sort((a: any, b: any) => {
      const valueA = a[key];
      const valueB = b[key];

      if (valueA < valueB) {
        return this.sortOrder.reverse ? 1 : -1;
      } else if (valueA > valueB) {
        return this.sortOrder.reverse ? -1 : 1;
      } else {
        return 0;
      }
    });
  }
  //#endregion
  //#region  get data
  private subscription!: Subscription;

  page: number = 1;
  count: number = 0;
  tableSize: number = 10;
  tableSizes: any = [10, 20, 50];
  POSTS: any;
  onTableDataChange(event: any) {
    this.page = event;

  }
  onTableSizeChange(event: any): void {
    this.tableSize = event.target.value;
    this.page = 1;

  }
  //#endregion

  ngOnInit(): void {
    this.subscription = this.service.data$.subscribe(data => {
      this.POSTS = data;
    });

    // Khi component được khởi tạo, fetch dữ liệu lần đầu
    this.service.fetchData();
  }


  //#region  delete product
  processDelete(item: any) {
    Swal.fire({
      title: "Are you sure?",
      text: "You won't be able to revert this!",
      icon: "warning",
      showCancelButton: true,
      confirmButtonColor: "#3085d6",
      cancelButtonColor: "#d33",
      confirmButtonText: "Yes, delete it!"
    }).then((result) => {
      if (result.isConfirmed) {
        this.service.deleteProduct(item).subscribe(res => {
          Swal.fire({
            title: "Deleted!",
            text: "Đã xóa thành công sản phẩm.",
            icon: "success"
          });
          this.service.fetchData();
        }, error => {
          console.log(error)
          Swal.fire({
            title: "Error!",
            text: "Có lỗi xảy ra trong quá trình thực hiện xóa.",
            icon: "error"
          });
        });

      }
    });
  }
  //#endregion

  //#region update product
  openDialogUpdate(data: any) {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.data = data;
    dialogConfig.width = '1200px';
    dialogConfig.autoFocus = false;
    dialogConfig.disableClose = true;
    dialogConfig.height = '750px';
    const dialogRef = this.dialog.open(ProductUpdateComponent, dialogConfig);

    dialogRef.afterClosed().subscribe(result => {
      console.log('clode update form');
    });
  }
  //#endregion

  //#region add product
  openDialogAddProduct() {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.width = '1200px';
    dialogConfig.autoFocus = false;
    dialogConfig.disableClose = true;
    dialogConfig.height = '750px';
    const diaRef = this.dialog.open(ProductAddComponent, dialogConfig);

    diaRef.afterClosed().subscribe(rs => {
      console.log('close add new product');
    })
  }
  //#endregion


  ngOnDestroy(): void {
    //Called once, before the instance is destroyed.
    //Add 'implements OnDestroy' to the class.
    if (this.subscription) {
      this.subscription.unsubscribe();
      console.log('destroy');
    }

  }

}
