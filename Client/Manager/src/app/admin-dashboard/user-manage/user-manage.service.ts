import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UserManageService {


  constructor(private http: HttpClient) { }
  getUsersData(): Observable<any> {
    const options = {
      headers: new HttpHeaders().append("Authorization", "Bearer " + this.getToken()),
    }
    return this.http.get(environment.apiUrl + '/api/list-user-detail', options).pipe(
      map((res: any) => res.response.data.usersInfor)
    )
  }

  getToken() {
    // Lấy token từ session
    return sessionStorage.getItem("token_admin");
  }


  deleteUser(userId: string): Observable<any> {
    const options = {
      headers: new HttpHeaders().append("Authorization", "Bearer " + this.getToken()),
    }
    return this.http.delete(environment.apiUrl + '/api/delete-user?userId=' + userId, options)
  }

  updateUser(userId: string, statusNew: number, active: boolean) {
    const options = {
      headers: new HttpHeaders().append("Authorization", "Bearer " + this.getToken()),
    }
    return this.http.post(environment.apiUrl + '/api/update-user?userId=' + userId + '&role=' + statusNew + '&active=' + active, {}, options)
  }

}
