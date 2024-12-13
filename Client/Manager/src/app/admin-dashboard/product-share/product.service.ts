import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, map } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  private dataSubject = new BehaviorSubject<any>(null);
  public data$ = this.dataSubject.asObservable();

  constructor(private http: HttpClient) { }

  fetchData() {
    this.getListProudct().subscribe(
      (data) => {
        this.dataSubject.next(data);
      },
      (error) => {
        console.error('Error fetching data:', error);
      }
    );
  }

  createProduct(item: any): Observable<any> {
    const options = {
      headers: new HttpHeaders().append("Authorization", "Bearer " + this.getToken()),
    }
    return this.http.post(environment.apiUrl + '/api/create-product', item, options)
  }

  updateProduct(item: any): Observable<any> {
    const options = {
      headers: new HttpHeaders().append("Authorization", "Bearer " + this.getToken()),
    }
    return this.http.put(environment.apiUrl + '/api/update-product', item, options)
  }


  deleteProduct(productId: any): Observable<any>{
    const options = {
      headers: new HttpHeaders().append("Authorization", "Bearer " + this.getToken()),
    }
    return this.http.delete(environment.apiUrl + '/api/delete-product?productId='+productId, options);
  }

  getToken() {
    // Lấy token từ session
    return sessionStorage.getItem("token_admin");
  }

  getListProudct(): Observable<any> {
    return this.http.get(environment.apiUrl + '/api/get-product').pipe(
      map((res: any) => res.response.data.listProductDeatails)
    )
  }


}
