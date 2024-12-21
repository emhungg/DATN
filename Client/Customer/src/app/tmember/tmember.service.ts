import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { Observable, map } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class TmemberService {

  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }


  getToken() {
    // Lấy token từ session
    return sessionStorage.getItem("token");
  }

  getUserById(): Observable<any> {
    const options = {
      headers: new HttpHeaders().append("Authorization", "Bearer " + this.getToken()),
    }
    return this.http.get(this.apiUrl + '/api/get-user-by-id', options).pipe(
      map((data: any) => data.response.data.user)
    );
  }

   sharedData: any;
  setUserInfor(data:any){
    this.sharedData = data;
  }
  getUserInfor() {
    return this.sharedData;
  }


  changeNameGender(value: FormGroup): Observable<any> {
    const options = {
      headers: new HttpHeaders().append("Authorization", "Bearer " + this.getToken()),
    }
    return this.http.post(environment.apiUrl + '/api/change-name-gender', value, options)
  }

  changePassword(value: FormGroup): Observable<any> {
    const options = {
      headers: new HttpHeaders().append("Authorization", "Bearer " + this.getToken()),
    }
    return this.http.post(environment.apiUrl + '/api/change-password', value, options)
  }

  changeAddress(value: FormGroup): Observable<any> {
    const options = {
      headers: new HttpHeaders().append("Authorization", "Bearer " + this.getToken()),
    }
    return this.http.post(environment.apiUrl + '/api/change-address', value, options)
  }
}
