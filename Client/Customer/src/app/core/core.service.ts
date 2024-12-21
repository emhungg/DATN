import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CoreService {

  constructor(private http: HttpClient) { }
  baseUrl = environment.apiUrl;
  sendMailUserRegisterAds(email: string) {
    return this.http.post(`${this.baseUrl}/api/register-mail?email=` + email, {});
  }
}
