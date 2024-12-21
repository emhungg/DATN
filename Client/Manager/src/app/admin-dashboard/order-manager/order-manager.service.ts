import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class OrderManagerService {

  constructor(private http: HttpClient) { }
  getOrderData(): Observable<any> {
    const options = {
      headers: new HttpHeaders().append("Authorization", "Bearer " + this.getToken()),
    }
    return this.http.get(environment.apiUrl + '/api/order-list', options).pipe(
      map((res: any) => res.response.data.orders)
    )
  }

  getToken() {
    // Lấy token từ session
    return sessionStorage.getItem("token_admin");
  }


  deleteOrderItem(orderId: string): Observable<any> {
    const options = {
      headers: new HttpHeaders().append("Authorization", "Bearer " + this.getToken()),
    }
    return this.http.delete(environment.apiUrl + '/api/detele-order?orderId=' + orderId, options).pipe(map((item: any) => item.response.data))
  }

  updateStatusOrder(orderId: string, statusNew: string, statusReceive: number) {
    const options = {
      headers: new HttpHeaders().append("Authorization", "Bearer " + this.getToken()),
    }
    return this.http.post(environment.apiUrl + '/api/update-status-order?orderId=' + orderId + '&statusNew=' + statusNew + '&statusReceive=' + statusReceive, {}, options)
  }

}
