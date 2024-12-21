import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class RegisterService {

  constructor(private http: HttpClient) {

  }

  apiUrl = environment.apiUrl + "/api/register-user";
  reisterProcessService(request: any) {
    return this.http.post(this.apiUrl, request);
  }

}
