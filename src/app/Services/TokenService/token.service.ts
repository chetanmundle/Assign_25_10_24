import { Injectable } from '@angular/core';
import { jwtDecode } from 'jwt-decode';

@Injectable({
  providedIn: 'root',
})
export class TokenService {
  getUserIdFromToken() {
    const token = localStorage.getItem('access_token');
    const decodedToken: any = jwtDecode(token ?? '');
    const userId = decodedToken.UserId;
    return userId;
  }
}
