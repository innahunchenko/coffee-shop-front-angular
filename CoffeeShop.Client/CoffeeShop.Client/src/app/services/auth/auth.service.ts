import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { BehaviorSubject, Observable, catchError, map, of, tap } from "rxjs";
import { User } from "../../models/auth/user.interface";
import { MenuItem } from "../../models/auth/menu-item.interface";

const API_BASE_URL = 'https://localhost:7075/auth';
const registerUrl = `${API_BASE_URL}/user/register`;
const loginUrl = `${API_BASE_URL}/user/login`;
const logoutUrl = `${API_BASE_URL}/user/logout`;
const authStatusUrl = `${API_BASE_URL}/user/check-auth-status`;
const userNameUrl = `${API_BASE_URL}/user/username`;
const menuUrl = `${API_BASE_URL}/user/menu`;
const isUserAdminUrl = `${API_BASE_URL}/user/is-user-admin`;
const userRoleUrl = `${API_BASE_URL}/user/role`;
const resetPasswordUrl = `${API_BASE_URL}/user/reset-password`;
const forgotPasswordUrl = `${API_BASE_URL}/user/forgot-password`;

@Injectable()
export class AuthService {
  private isLoggedInSubject = new BehaviorSubject<boolean>(false);
  isLoggedIn$ = this.isLoggedInSubject.asObservable();
  resetToken: string = "";
  constructor(private http: HttpClient) { }

  forgotPassword(email: string): Observable<string> {
    return this.http.post<string>(forgotPasswordUrl, { email });
  }

  resetPassword(email: string, token: string, password: string, confirmPassword: string): Observable<void> {
    return this.http.post<void>(resetPasswordUrl, { email, token, password, confirmPassword });
  }

  register(userData: User): Observable<any> {
    return this.http.post(registerUrl, userData);
  }

  login(credentials: User): Observable<any> {
    return this.http.post(loginUrl, credentials).pipe(
      tap(() => {
        this.isLoggedInSubject.next(true);
      })
    );
  }

  isAuthenticated(): Observable<boolean> {
    return this.http.get<boolean>(authStatusUrl).pipe(
      tap((isAuthenticated: boolean) => {
        this.isLoggedInSubject.next(isAuthenticated);
      }),
      catchError(() => {
        this.isLoggedInSubject.next(false);
        return of(false);
      })
    );
  }

  logout(): Observable<void> {
    return this.http.post<void>(logoutUrl, {}).pipe(
      tap(() => {
        this.isLoggedInSubject.next(false);
      })
    );
  }

  getUserName(): Observable<string> {
    return this.http.get<{ username: string }>(userNameUrl).pipe(
      map(response => response.username)
    );
  }

  getMenu(): Observable<MenuItem[]> {
    return this.http.get<MenuItem[]>(menuUrl);
  }

  isUserAdmin(): Observable<boolean> {
    return this.http.get<boolean>(isUserAdminUrl);
  }

  getUserRole(): Observable<string> {
    return this.http.get<{ role: string }>(userRoleUrl).pipe(
      map(response => response.role)
    );
  }
}
