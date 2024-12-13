import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, map } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class NewsManageService {
  private dataSubject = new BehaviorSubject<any>(null);
  public data$ = this.dataSubject.asObservable();
  constructor(private http: HttpClient) { }


  fetchData() {
    this.getListNews().subscribe(
      (data) => {
        this.dataSubject.next(data);
      },
      (error) => {
        console.error('Error fetching data:', error);
      }
    );
  }

  getListNews(): Observable<any> {
    return this.http.get(environment.apiUrl + '/api/get-news').pipe(
      map((item: any) => item.response.data.newsData)
    )
  }

  getToken() {
    // Lấy token từ session
    return sessionStorage.getItem("token_admin");
  }

  createNews(data: FormData): Observable<any> {
    const options = {
      headers: new HttpHeaders().append("Authorization", "Bearer " + this.getToken()),
    }
    return this.http.post(environment.apiUrl + '/api/create-news', data, options)
  }

  processDeteleNew(id: number): Observable<any> {
    const options = {
      headers: new HttpHeaders().append("Authorization", "Bearer " + this.getToken()),
    }
    return this.http.delete(environment.apiUrl + '/api/detele-news?id=' + id, options).pipe(
      map((item: any) => item.response.data)
    )
  }

  getNewsById(id: number): Observable<any> {
    return this.http.get(environment.apiUrl + "/api/get-news-detail?id=" + id).pipe(
      map((item: any) => item.response.data)
    )
  }

  updateNews(data: FormData, id: number): Observable<any> {
    const options = {
      headers: new HttpHeaders().append("Authorization", "Bearer " + this.getToken()),
    }
    return this.http.put(environment.apiUrl + '/api/update-news?id=' + id, data, options)
  }

}
