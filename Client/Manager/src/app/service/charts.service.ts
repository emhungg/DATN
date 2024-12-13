import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ChartsService {

  constructor(private http: HttpClient) { }
  getToken() {
    // Lấy token từ session
    return sessionStorage.getItem("token_admin");
  }
  getDataToShow(month: number): Observable<any> {
    const options = {
      headers: new HttpHeaders().append("Authorization", "Bearer " + this.getToken()),
    }
    return this.http.get(environment.apiUrl + '/api/get-statistic?month=' + month, options).pipe(
      map((item: any) => item.response.data)
    )
  }
}
