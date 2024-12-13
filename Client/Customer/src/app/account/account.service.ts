import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ReplaySubject, map, of } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  constructor(private httpRequest: HttpClient) { }

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
    return sessionStorage.getItem("token");
  }

  sendCodeProcess() {
    const options = {
      headers: new HttpHeaders().append("Authorization", "Bearer " + this.getToken()),
    }
    return this.httpRequest.get(environment.apiUrl + "/api/send-mail", options);
  }

  processValidateCode(code: any) {
    const options = {
      headers: new HttpHeaders().append("Authorization", "Bearer " + this.getToken()),
    }
    return this.httpRequest.post(environment.apiUrl + "/api/verify-email?code=" + code, {}, options);
  }

  // isUserLoggedIn
  isUserLoggedIn(): boolean {
    if (this.getToken() && sessionStorage.getItem('isActive')) {
      return true;
    }
    return false;
  }


  apiUrlRegister = environment.apiUrl + "/api/register-user";
  reisterProcessService(request: any) {
    return this.httpRequest.post(this.apiUrlRegister, request);
  }

}
