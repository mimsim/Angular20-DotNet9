import { inject, Service, signal } from '@angular/core';
import { AuthUser, LoginRequest, LoginResponse, RegisterRequest, RegisterResponse } from '../interfaces/user.interfaces';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { LikesService } from './likes-service';

@Service()
export class AuthService {
    private readonly http = inject(HttpClient);
    public likesService = inject(LikesService);
    private readonly apiUrl = 'http://localhost:5001/api/account/login';

    private readonly registerUrl = 'http://localhost:5001/api/account/register';
    readonly currentUser = signal<AuthUser | null>(this.readUserFromStorage());

    login(credentials: LoginRequest): Observable<AuthUser> {
        return this.http.post<AuthUser>(this.apiUrl, credentials).pipe(
            tap((user) => this.setCurrentUser(user))
        );
    }

    register(data: RegisterRequest): Observable<AuthUser> {
        return this.http.post<AuthUser>(this.registerUrl, data).pipe(
            tap((user) => this.setCurrentUser(user))
        );
    }

    logout(): void {
        this.currentUser.set(null);
        localStorage.removeItem('currentUser');
    }

    private setCurrentUser(user: AuthUser): void {
        this.currentUser.set(user);
        localStorage.setItem('currentUser', JSON.stringify(user));
        this.likesService.getLikeIds();
    }

    private readUserFromStorage(): AuthUser | null {
        const raw = localStorage.getItem('currentUser');
        if (!raw) return null;
        try {
            return JSON.parse(raw) as AuthUser;
        } catch {
            return null;
        }
    }


}
