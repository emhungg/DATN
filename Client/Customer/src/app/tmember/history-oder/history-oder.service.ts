import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import { environment } from 'src/environments/environment';
import { TmemberService } from '../tmember.service';

@Injectable({
  providedIn: 'root'
})
export class HistoryOderService {

  constructor(private http: HttpClient, private userInfo: TmemberService) { }

  getToken() {
    // Lấy token từ session
    return sessionStorage.getItem("token");
  }

  getHistoryInfor(): Observable<any> {
    const options = {
      headers: new HttpHeaders().append("Authorization", "Bearer " + this.getToken()),
    }
    return this.http.get(environment.apiUrl + '/api/customer-get-order', options).pipe(
      map((item: any) => item.response.data)
    )
  }

  getUserInfor() {
    return this.userInfo.getUserInfor();
  }

  getOrderDetail(orderId: string): Observable<any> {
    const options = {
      headers: new HttpHeaders().append("Authorization", "Bearer " + this.getToken()),
    }
    return this.http.get(environment.apiUrl + '/api/get-order-details?orderId=' + orderId, options).pipe(
      map((item: any) => item.response.data.data)
    )
  }
}
