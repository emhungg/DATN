import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

export const authRoleGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);
  const userRole = sessionStorage.getItem('roles');
  const role = route.data['roles'];
  // Kiểm tra vai trò của người dùng
  if (role.includes(userRole)) {
    return true; // Người dùng bình thường có quyền truy cập
  }  
  // Nếu không thỏa mãn bất kỳ vai trò nào, chuyển hướng đến trang không có quyền truy cập
  router.navigate(['']);
  return false;

};
