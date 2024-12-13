import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class NewsService {

  constructor(private http: HttpClient) { }

  getListNews(): Observable<any> {
    return this.http.get(environment.apiUrl + '/api/get-news').pipe(
      map((item: any) => item.response.data.newsData)
    )
  }

  getNewsById(id: number): Observable<any> {
    return this.http.get(environment.apiUrl + "/api/get-news-detail?id=" + id).pipe(
      map((item: any) => item.response.data)
    )
  }
}
