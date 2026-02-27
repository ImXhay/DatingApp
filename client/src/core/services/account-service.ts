import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { LoginCreds, RegistrCreds, User } from '../../types/user';
import { tap } from 'rxjs';
import { JsonPipe } from '@angular/common';
import { environment } from '../../environments/environment';
import { LikeService } from './like-service';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  private http = inject(HttpClient);
  private likeService = inject(LikeService);
  currentUser = signal<User | null>(null);

  private baseUrl = environment.apiUrl;

  register(creds: RegistrCreds) {
    return this.http
      .post<User>(this.baseUrl + 'account/register', creds, { withCredentials: true })
      .pipe(
        tap((user) => {
          if (user) {
            this.setCurrentUser(user);
            this.startTokenRefreshInterval();
          }
        }),
      );
    }

  login(creds: LoginCreds) {
    return this.http
      .post<User>(this.baseUrl + 'account/login', creds, { withCredentials: true })
      .pipe(
        tap((user) => {
          if (user) {
            this.setCurrentUser(user);
            this.startTokenRefreshInterval();
          }
        }),
      );
  }

  refreshToken() {
    return this.http.post<User>(
      this.baseUrl + 'account/refresh-token',
      {},
      { withCredentials: true },
    );
  }

  startTokenRefreshInterval() {
    setInterval(() => {
    this.http.post<User>(
      this.baseUrl + 'account/refresh-token',
      {},
      {withCredentials: true}).subscribe({
        next: user => {
          this.setCurrentUser(user)
        },
        error: () => {
          this.logout()
        }
      })
      }, 5 * 60 * 1000)
  }

  setCurrentUser(user: User) {
    user.roles = this.getRolesFromUser(user);
    this.currentUser.set(user);
    this.likeService.getLikeIds();
  }

  logout() {
    localStorage.removeItem('filters');
    this.likeService.clearLikeIds();
    this.currentUser.set(null);
  }

  private getRolesFromUser(user: User): string[] {
    const payload = user.token.split('.')[1];
    const decoded = atob(payload);
    const jsonPayload = JSON.parse(decoded);
    return Array.isArray(jsonPayload.role) ? jsonPayload.role : [jsonPayload.role];
  }
}
