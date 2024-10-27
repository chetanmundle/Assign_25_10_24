import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { jwtDecode } from 'jwt-decode';

export const authGuard: CanActivateFn = (route, state) => {
  const token = localStorage.getItem('access_token');

  const router = inject(Router);

  if (token) {
    try {
      const decodedToken: any = jwtDecode(token);
      const currentTime = Math.floor(Date.now() / 1000);

      if (decodedToken.exp > currentTime) {
        
        // const userId = decodedToken.UserId; // <-- Get the userId from the decoded token
        // console.log('User ID:', userId); // You can use the userId as needed

        return true; // Token is valid
      } else {
        // Token is expired
        localStorage.removeItem('access_token');
        router.navigate(['/Login']);
        return false;
      }
    } catch (error) {
      console.error('Invalid token:', error);
      localStorage.removeItem('access_token');
      router.navigate(['/Login']);
      return false;
    }
  } else {
    router.navigateByUrl('/Login');
    return false;
  }
};
