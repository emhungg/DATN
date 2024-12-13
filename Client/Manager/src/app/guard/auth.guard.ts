import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';


export const authGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);
  const token = sessionStorage.getItem('token_admin');
  if (token) {
    return true;
  } else {
    router.navigate(['']);
    return false;
  }
};

