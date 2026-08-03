import { inject, Service } from '@angular/core';
import { LoginRequest, LoginResponse, RegisterRequest, RegisterResponse } from '../interfaces/user.interfaces';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Service()
export class AuthService {
    private readonly http = inject(HttpClient);

       private readonly apiUrl = 'http://localhost:5001/api/account/login';
  
    private readonly registerUrl = 'http://localhost:5001/api/account/register';

    login(credentials: LoginRequest): Observable<LoginResponse> {
        return this.http.post<LoginResponse>(this.apiUrl, credentials);
    }

    register(data: RegisterRequest): Observable<RegisterResponse> {
        return this.http.post<RegisterResponse>(this.registerUrl, data);
    }


}
