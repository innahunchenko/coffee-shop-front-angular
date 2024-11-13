import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable, map } from "rxjs";
import { User } from "../../models/auth/user.interface";
import { MenuItem } from "../../models/auth/menu-item.interface";

const API_BASE_URL = 'https://localhost:7075/auth';
const registerUrl = `${API_BASE_URL}/user/register`;
const loginUrl = `${API_BASE_URL}/user/login`;
const authStatusUrl = `${API_BASE_URL}/user/check-auth-status`;
const userNameUrl = `${API_BASE_URL}/user/username`;
const menuUrl = `${API_BASE_URL}/user/menu`;

@Injectable()
export class AuthService {
  constructor(private http: HttpClient) { }

  register(userData: User): Observable<any> {
    return this.http.post(registerUrl, userData);
  }

  login(credentials: User): Observable<any> {
    return this.http.post(loginUrl, credentials);
  }

  isAuthenticated(): Observable<boolean> {
    return this.http.get<{ isAuthenticated: boolean }>(authStatusUrl)
      .pipe(
        map(response => response.isAuthenticated)
      );
  }

  getUserName(): Observable<string> {
    return this.http.get<{ userName: string }>(userNameUrl).pipe(
      map(response => response.userName)
    );
  }

  getMenu(): Observable<MenuItem[]> {
    return this.http.get<MenuItem[]>(menuUrl);
  }
}
