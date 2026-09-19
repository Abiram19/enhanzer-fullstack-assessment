import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import {
  LoginRequest,
  LoginResponse,
  LocationDto
} from '../models/login.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly apiUrl =
    'https://enhanzer-fullstack-assessment.onrender.com/api/auth/login';

  private readonly localApiUrl =
    'https://enhanzer-fullstack-assessment.onrender.com/api/locations';

  private currentUserCompanyCode: string | null = null;

  constructor(private http: HttpClient) { }

  get isAuthenticated(): boolean {
    return this.currentUserCompanyCode !== null;
  }

  getCompanyCode(): string | null {
    return this.currentUserCompanyCode;
  }

  setAuthenticatedUser(companyCode: string): void {
    this.currentUserCompanyCode = companyCode;
  }

  logout(): void {
    this.currentUserCompanyCode = null;
  }

  login(credentials: LoginRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(
      this.apiUrl,
      credentials
    );
  }
}