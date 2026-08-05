import { HttpClient } from '@angular/common/http';
import { inject, Service } from '@angular/core';
import { Observable } from 'rxjs';
import { IMember } from '../interfaces/members.interfaces';

@Service()
export class MembersService {
    private http = inject(HttpClient);
    private baseUrl = 'http://localhost:5001/api/members'; // смени с твоя реален URL

    getMembers(): Observable<IMember[]> {
        return this.http.get<IMember[]>(this.baseUrl,this.getHttpOptions());
    }
    getMemberById(id: string) {
        return this.http.get<IMember>(`${this.baseUrl}/${id}`, this.getHttpOptions());
    }
    private getHttpOptions() {
        const token = localStorage.getItem('currentUser');
        return {
            headers: {
                Authorization: `Bearer ${token}`,
            },
        };
    }
    updateMember(member: Partial<IMember>) {
        return this.http.put(this.baseUrl, member, this.getHttpOptions());
    }

}
