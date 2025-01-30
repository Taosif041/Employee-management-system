import { CanActivateFn } from '@angular/router';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth/auth.service';
import { SharedService } from '../services/shared/shared.service';

export const authGuard: CanActivateFn = (route, state) => {
  const _authService = inject(AuthService);
  const router = inject(Router);
  const _sharedService = inject(SharedService);

  const isLoggedIn = _authService.isAuthenticated();

  if (!isLoggedIn) {
    _sharedService.openSnackBar('Login required', 'navigate', false);
    router.navigate(['/login'], { queryParams: { returnUrl: state.url } });
    return false;
  }

  const requiredRoles = route.data['roles'] as Array<number>;

  const userRoles = _authService.getUserRoles();
  console.log('userRoles -->', userRoles);

  if (!userRoles || !requiredRoles.some((role) => userRoles.includes(role))) {
    _sharedService.openSnackBar('Authorization denied', 'required', false);
    router.navigate(['/employee']);
    return false;
  }

  return true;
};
