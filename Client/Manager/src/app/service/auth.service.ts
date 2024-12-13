import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import Swal from 'sweetalert2';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private httpRequest: HttpClient, private toastr: ToastrService) { }

  apiUrl = environment.apiUrl + "/api/login";

  apiUrlLogout = environment.apiUrl + "/api/logout";
  //login
  doLogin(request: any) {
    return this.httpRequest.post(this.apiUrl, request);
  }

  //logout
  logout() {
    const options = {
      headers: new HttpHeaders().append("Authorization", "Bearer " + this.getToken()),
    }
    return this.httpRequest.post(this.apiUrlLogout, {}, options);
  }

  getToken() {
    // Lấy token từ session
    return sessionStorage.getItem("token_admin");
  }

  // isUserLoggedIn
  isUserLoggedIn(): boolean {
    if (this.getToken()) {
      return true;
    }
    return false;
  }


  // Is user onl
  isUserOnl(): Observable<any> {
    const options = {
      headers: new HttpHeaders().append("Authorization", "Bearer " + this.getToken()),
    }
    return this.httpRequest.put(environment.apiUrl + "/api/user-onl", {}, options);
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
          this.logout().subscribe(res => {
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
