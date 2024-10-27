import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private http = inject(HttpClient);

  private Url = `https://localhost:7047/api/User`;

  createUser(user: any): Observable<any> {
    return this.http.post<any>(`${this.Url}/CreateUser`, user);
  }

  loginUser(loginData: any): Observable<any> {
    return this.http.post<any>(`${this.Url}/LoginUser`, loginData);
  }

  forgetPassword(fogetDto: any): Observable<any> {
    return this.http.put<any>(`${this.Url}/UpdatePassword`, fogetDto);
  }
}
