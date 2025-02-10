import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { BehaviorSubject, Observable, catchError, map, of, tap } from "rxjs";
import { User } from "../../models/auth/user.interface";
import { MenuItem } from "../../models/auth/menu-item.interface";
import { ENVIRONMENT } from "../../../environments/environment";

const apiGatewayUrl = ENVIRONMENT.apiGatewayUrl;
const baseUrl = `${apiGatewayUrl}/auth`;
const registerUrl = `${baseUrl}/user/register`;
const loginUrl = `${baseUrl}/user/login`;
const logoutUrl = `${baseUrl}/user/logout`;
const authStatusUrl = `${baseUrl}/user/check-auth-status`;
const userNameUrl = `${baseUrl}/user/username`;
const menuUrl = `${baseUrl}/user/menu`;
const isUserAdminUrl = `${baseUrl}/user/is-user-admin`;
const userRoleUrl = `${baseUrl}/user/role`;
const resetPasswordUrl = `${baseUrl}/user/reset-password`;
const forgotPasswordUrl = `${baseUrl}/user/forgot-password`;

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
