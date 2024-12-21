import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, map, of } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  /**
   *
   */
  constructor(private http: HttpClient) {

  }

  getProductByCategoryId(cId: string): Observable<any> {
    const options = {
      headers: new HttpHeaders().append("Authorization", "Bearer " + this.getToken()),
    }
    return this.http.get(environment.apiUrl + '/api/product-by-category?request=' + cId, options).pipe(
      map((item: any) => item.response.data.listProductDeatails)
    )
  }

  getToken() {
    // Lấy token từ session
    return sessionStorage.getItem("token");
  }
  getProduct(): Observable<any> {
    const options = {
      headers: new HttpHeaders().append("Authorization", "Bearer " + this.getToken()),
    }
    return this.http.get(environment.apiUrl + '/api/get-product', options).pipe(
      map((item: any) => item.response.data.listProductDeatails)
    )
  }

  getProductDetail(item: any): Observable<any> {
    const options = {
      headers: new HttpHeaders().append("Authorization", "Bearer " + this.getToken()),
    }
    return this.http.get(environment.apiUrl + '/api/product-details?productId=' + item, options).pipe(
      map((item: any) => item.response.data)
    )
  }
  getViewedProduct(item: any): Observable<any> {
    const options = {
      headers: new HttpHeaders().append("Authorization", "Bearer " + this.getToken()),
    }
    return this.http.post(environment.apiUrl + '/api/viewed-product', item, options).pipe(
      map((item: any) => item.response.data.listProductDeatails)
    )
  }

  ChangeWishPoduct(productId: string) {
    const options = {
      headers: new HttpHeaders().append("Authorization", "Bearer " + this.getToken()),
    }
    return this.http.post(environment.apiUrl + '/api/wish-product?productId=' + productId, {}, options).pipe(
      map((item: any) => item.response.data)
    )
  }

  getWishList() {
    const options = {
      headers: new HttpHeaders().append("Authorization", "Bearer " + this.getToken()),
    }
    return this.http.get(environment.apiUrl + '/api/get-wish-list', options).pipe(
      map((item: any) => item.response.data.listProductDeatails)
    )
  }

  processRating(infor: any): Observable<any> {
    const options = {
      headers: new HttpHeaders().append("Authorization", "Bearer " + this.getToken()),
    }
    return this.http.post(environment.apiUrl + '/api/add-comment-rating', infor, options)

  }

  getratingByProductID(productId: string): Observable<any> {
    return this.http.get(environment.apiUrl + '/api/get-comment-rating?productId=' + productId).pipe(
      map((item:any) => item.response.data)
    )
  }

}