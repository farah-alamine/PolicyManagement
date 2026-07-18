import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { LoginRequest } from '../models/login-request.model';
import { LoginResponse } from '../models/login-response.model';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  private readonly http = inject(HttpClient);

private readonly apiUrl =
    `${environment.apiUrl}/auth`;
  private readonly tokenKey = 'policy_management_token';
  private readonly userKey = 'policy_management_user';

  currentUser = signal<LoginResponse | null>(this.getStoredUser());

  login(request: LoginRequest): Observable<LoginResponse> {
    return this.http
      .post<LoginResponse>(`${this.apiUrl}/login`, request)
      .pipe(
        tap(response => {
          this.storeAuthentication(response);
        })
      );
  }

  logout(): void {
    localStorage.removeItem(this.tokenKey);
    localStorage.removeItem(this.userKey);

    this.currentUser.set(null);
  }

  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  isAuthenticated(): boolean {
    const token = this.getToken();

    if (!token) {
      return false;
    }

    const user = this.currentUser();

    if (!user) {
      return false;
    }

    const expiryDate = new Date(user.expiresAt);

    return expiryDate.getTime() > Date.now();
  }

  private storeAuthentication(response: LoginResponse): void {
    localStorage.setItem(this.tokenKey, response.token);
    localStorage.setItem(this.userKey, JSON.stringify(response));

    this.currentUser.set(response);
  }

  private getStoredUser(): LoginResponse | null {
    const storedUser = localStorage.getItem(this.userKey);

    if (!storedUser) {
      return null;
    }

    try {
      return JSON.parse(storedUser) as LoginResponse;
    } catch {
      localStorage.removeItem(this.userKey);
      localStorage.removeItem(this.tokenKey);

      return null;
    }
  }
}