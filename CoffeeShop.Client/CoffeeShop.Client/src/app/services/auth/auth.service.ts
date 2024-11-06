import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { User } from "../../models/auth/user.interface";

const API_BASE_URL = 'https://localhost:7075/auth';
const registerUrl = `${API_BASE_URL}/user/register`;
const loginUrl = `${API_BASE_URL}/user/login`;

@Injectable()
export class AuthService {
  constructor(private http: HttpClient) { }

  register(userData: User): Observable<any> {
    return this.http.post(registerUrl, userData);
  }

  login(credentials: User): Observable<any> {
    return this.http.post(loginUrl, credentials);
  }
}
