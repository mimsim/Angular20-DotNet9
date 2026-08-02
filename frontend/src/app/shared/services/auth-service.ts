import { inject, Service } from '@angular/core';
import { LoginRequest, LoginResponse } from '../interfaces/user.interfaces';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Service()
export class AuthService {
    private readonly http = inject(HttpClient);

    // замести с реалния endpoint на твоя backend
    private readonly apiUrl = 'http://localhost:5001/api/account/login';

    login(credentials: LoginRequest): Observable<LoginResponse> {
        return this.http.post<LoginResponse>(this.apiUrl, credentials);
    }

}
