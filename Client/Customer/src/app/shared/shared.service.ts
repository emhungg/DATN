import { Observable, map } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { ImgData } from '../Models/img-data';

@Injectable({
  providedIn: 'root'
})
export class SharedService {

  constructor(private http: HttpClient) { }
  baseurl = environment.apiUrl;
  getImgsSupplier(): Observable<any> {
    return this.http.get<any>(this.baseurl + '/api/supplier-img').pipe(
      map(response => response.response.data.imgs.map((imgData: ImgData) => imgData.image))
    );
  }

  getDataBanner(): Observable<any> {
    return this.http.get(environment.apiUrl + '/api/get-data-banner').pipe(
      map((item: any) => item.response.data.data)
    )
  }
}
