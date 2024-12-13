import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

export const authIsactiveGuard: CanActivateFn = (route, state) => {
  const isActive = sessionStorage.getItem('isActive');
  const router = inject(Router);
  if (isActive) {
    return true;
  } else {
    router.navigate(['verify-email']);
    return false;
  }
};
