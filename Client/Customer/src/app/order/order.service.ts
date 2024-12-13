import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class OrderService {

  constructor(private http: HttpClient) { }

  convertPriceToNumver(price: number): Observable<any> {
    return this.http.get(environment.apiUrl + '/api/price-number?price=' + price).pipe(
      map((words: any) => words.response.data)
    )
  }


  // params
  // {
  //   "userName": "string",
  //   "phoneNumber": "string",
  //   "email": "string",
  //   "receiveType": 0,
  //   "province": "string",
  //   "addressReceive": "string",
  //   "orderItemInsertData": [
  //     {
  //       "productId": "string",
  //       "quantity": 0,
  //       "price": 0
  //     }
  //   ],
  //   "note": "string",
  //   "methodPayment": "string",
  //   "totalAmount": 0
  // }
  processCreatedOrder(item: any): Observable<any> {
    const options = {
      headers: new HttpHeaders().append("Authorization", "Bearer " + this.getToken()),
    }
    return this.http.post(environment.apiUrl + '/api/created-order', item, options).pipe(
      map((item: any) => item.response.data)
    )
  }


  verifyOrder(item: any): Observable<any> {
    return this.http.post(environment.apiUrl + '/api/vnpay_return/IPN', item).pipe(
      map((item:any)=> item.response.data)
    )
  }

  getToken() {
    // Lấy token từ session
    return sessionStorage.getItem("token");
  }

}
