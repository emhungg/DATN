import { Component, Input, SimpleChanges } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { filter } from 'rxjs';
import { AccountService } from 'src/app/account/account.service';

import Swal from 'sweetalert2';

@Component({
  selector: 'app-home-member',
  templateUrl: './home-member.component.html',
  styleUrls: ['./home-member.component.css']
})
export class HomeMemberComponent {
  currentUrl: any;
  constructor(private service: AccountService, private router: Router, private toastr: ToastrService) {
    
  }
  //#region  log out
  isLoggedIn: boolean = false;
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
  //#endregion

}
