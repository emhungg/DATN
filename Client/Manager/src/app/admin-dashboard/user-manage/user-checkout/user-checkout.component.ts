import { Component } from '@angular/core';
import Swal from 'sweetalert2';
import { OrderManagerService } from '../../order-manager/order-manager.service';
import { ToastrService } from 'ngx-toastr';
import { UserManageService } from '../user-manage.service';

@Component({
  selector: 'app-user-checkout',
  templateUrl: './user-checkout.component.html',
  styleUrls: ['./user-checkout.component.scss']
})
export class UserCheckoutComponent {
  userDatas!: any[];
  first = 0;
  rows = 10;
  userDialog: boolean = false;
  submitted: boolean = false;
  statuses!: any[];
  roles!: any[];
  clonedProducts: { [s: string]: any } = {};
  optionActive!: any[];
  constructor(private userServer: UserManageService, private toastr: ToastrService) { }

  ngOnInit() {
    this.userServer.getUsersData().subscribe((data) => {
      this.userDatas = data
      console.log(data)
    });

    this.statuses = [
      { label: 'Customer', value: 0 },
      { label: 'Admin', value: 1 }
    ];

    this.optionActive = [
      { label: 'Active', value: true },
      { label: 'InActive', value: false }
    ];
  }

  pageChange(event: any) {
    this.first = event.first;
    this.rows = event.rows;
  }


  extractInputValue(event: Event): string {
    if (event.target instanceof HTMLInputElement) {
      console.log(event.target.value)
      return event.target.value;
    }
    return '';
  }


  getSeverity(status: number) {
    switch (status) {
      case 0:
        return 'success';
      case 1:
        return 'info';
      default:
        return 'warning';
    }
  }
  getRoles(role: number) {
    switch (role) {
      case 0:
        return 'Customer';
      case 1:
        return 'Admin';
      default:
        return 'warning';
    }
  }

  getSeverityActive(status: boolean) {
    switch (status) {
      case true:
        return 'success';
      case false:
        return 'danger';
      default:
        return 'warning';
    }
  }

  getActive(status: boolean) {
    switch (status) {
      case true:
        return 'Active';
      case false:
        return 'InActive';
      default:
        return 'warning';
    }
  }


  deleteProduct(product: any) {
    Swal.fire({
      title: "Are you sure you want to delete " + product.userId + '?',
      icon: 'question',
      showDenyButton: true,
      confirmButtonText: 'Đồng ý',
      denyButtonText: 'Không'
    }).then((rs) => {
      if (rs.isConfirmed) {
        this.userServer.deleteUser(product.userId).subscribe(res => {
          this.userDatas = this.userDatas.filter((val) => val.userId !== product.userId);
          this.toastr.success('Delete successfuly!', 'Deleted');
        }, error => {
          this.toastr.error('error!', 'Deleted');
        })



      }
    })
  }



  onRowEditInit(user: any) {
    this.clonedProducts[user.userId as string] = { ...user };
  }

  onRowEditSave(user: any, index: number) {
    if (this.clonedProducts[user.userId as string].isActive !== user.isActive || this.clonedProducts[user.userId as string].role !== user.role) {

      this.userServer.updateUser(user.userId, user.role, user.isActive).subscribe(res => {
        delete this.clonedProducts[user.userId as string];
        this.toastr.success('Successful', 'Updated');
      }, error => {
        this.toastr.warning('Can\'t update this account', 'Error');
        this.userDatas[index] = this.clonedProducts[user.userId as string];
        delete this.clonedProducts[user.userId as string];
      })

    }
  }

  onRowEditCancel(product: any, index: number) {
    this.userDatas[index] = this.clonedProducts[product.userId as string];
    delete this.clonedProducts[product.userId as string];
  }

}
