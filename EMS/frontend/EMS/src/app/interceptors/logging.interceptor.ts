import { HttpHandlerFn, HttpInterceptorFn } from '@angular/common/http';
import { HttpRequest } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, Observable, switchMap, throwError } from 'rxjs';
import { AuthService } from '../services/auth/auth.service';

export const loggingInterceptor: HttpInterceptorFn = (
  req: HttpRequest<unknown>,
  next: HttpHandlerFn
): Observable<any> => {
  const authService = inject(AuthService);
  const authToken = authService.getToken();
  console.log('Logging Interceptor authToken --> ', authToken);

  const newReq = req.clone({
    headers: req.headers.set('Authorization', `Bearer ${authToken ?? ''}`),
  });

  return next(newReq).pipe(
    catchError((error) => {
      if (error.status === 401) {
        console.error('Token expired. Attempting refresh...');

        return authService.updateTokenUsingRefreshToken().pipe(
          switchMap((response) => {
            const refreshedReq = req.clone({
              headers: req.headers.set(
                'Authorization',
                `Bearer ${response.token}`
              ),
            });
            return next(refreshedReq);
          }),

          catchError((refreshError) => {
            console.error('Refresh token failed', refreshError);
            authService.resetToken();
            return throwError(refreshError);
          })
        );
      }
      return throwError(error);
    })
  );
};
