import { Injectable, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Response } from '../models/api-response.model';
import { LoginDTO, RegisterDTO, SessionDTO } from '../models/auth.models';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = `${environment.endpoint}/Auth`;
  private readonly SESSION_KEY = 'ERP_SESSION_USER';

  // State management using Angular 19 Signal
  public readonly currentUser = signal<SessionDTO | null>(this.getInitialSession());

  public login(credentials: LoginDTO): Observable<Response<SessionDTO>> {
    return this.http.post<Response<SessionDTO>>(`${this.apiUrl}/Login`, credentials).pipe(
      tap((res: Response<SessionDTO>) => {
        if (res.status && res.value) {
          this.setSession(res.value);
        }
      })
    );
  }

  public register(userData: RegisterDTO): Observable<Response<SessionDTO>> {
    return this.http.post<Response<SessionDTO>>(`${this.apiUrl}/Register`, userData).pipe(
      tap((res: Response<SessionDTO>) => {
        if (res.status && res.value) {
          this.setSession(res.value);
        }
      })
    );
  }

  public setSession(session: SessionDTO): void {
    localStorage.setItem(this.SESSION_KEY, JSON.stringify(session));
    this.currentUser.set(session);
  }

  public logout(): void {
    localStorage.removeItem(this.SESSION_KEY);
    this.currentUser.set(null);
  }

  public getToken(): string | null {
    return this.currentUser()?.token || null;
  }

  public isLoggedIn(): boolean {
    return !!this.currentUser()?.token;
  }

  private getInitialSession(): SessionDTO | null {
    try {
      const saved = localStorage.getItem(this.SESSION_KEY);
      return saved ? JSON.parse(saved) : null;
    } catch {
      return null;
    }
  }
}
