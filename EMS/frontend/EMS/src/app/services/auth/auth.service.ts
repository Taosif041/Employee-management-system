import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, tap } from 'rxjs';
import { SharedService } from '../shared/shared.service';
import { jwtDecode } from 'jwt-decode';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private apiUrl = 'http://localhost:5000/api/Auth';
  private router = inject(Router);

  private http = inject(HttpClient);

  private _sharedService = inject(SharedService);

  constructor() {}

  login(username: string, password: string): Observable<any> {
    const payload = { username, password };
    return this.http.post<any>(`${this.apiUrl}/login`, payload).pipe(
      tap((response) => {
        this.setToken(response.token, response.refreshToken);
      })
    );
  }

  setToken(token: string, refreshToken: string): void {
    localStorage.setItem('auth_token', token);
    localStorage.setItem('refresh_token', refreshToken);
  }
  resetToken(): void {
    localStorage.removeItem('auth_token');
    localStorage.removeItem('refresh_token');
  }

  getToken(): string | null {
    return localStorage.getItem('auth_token');
  }

  getRefreshToken(): string | null {
    return localStorage.getItem('refresh_token');
  }

  updateTokenUsingRefreshToken(): Observable<any> {
    const refreshToken = this.getRefreshToken();
    if (!refreshToken) {
      this._sharedService.openSnackBar('User', 'sessionExpired', false);
      this.router.navigate(['/login']);
    }
    const payload = { refreshToken: refreshToken };

    return this.http.post<any>(`${this.apiUrl}/refresh`, payload).pipe(
      tap((response) => {
        this.setToken(response.token, response.refreshToken);
      })
    );
  }

  getDecodedToken(): any {
    const token = this.getToken();
    if (!token) {
      return null;
    }
    try {
      return jwtDecode(token);
    } catch (error) {
      console.error('Invalid token', error);
      return null;
    }
  }

  getUserRoles(): number[] | null {
    const decodedToken = this.getDecodedToken();
    return decodedToken?.roles || null; // Assumes 'roles' is present in the payload
  }

  isAuthenticated(): boolean {
    return !!this.getToken();
  }
}
