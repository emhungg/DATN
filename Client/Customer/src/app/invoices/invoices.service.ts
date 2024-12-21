import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class InvoicesService {
  private apiUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }

  GenerateInvoicePDF(orderId: string): Observable<any> {
    return this.http.get(this.apiUrl + "/api/invoice-bill?orderID=" + orderId, { observe: 'response', responseType: 'blob' });
  }


  GenerateInvoicLink(orderId: string): Observable<any> {
    return this.http.get(this.apiUrl + "/api/generate-link?orderID=" + orderId).pipe(
      map((response: any) => response.response.data)
    );
  }

}
