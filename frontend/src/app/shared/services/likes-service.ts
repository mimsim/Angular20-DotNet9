import { HttpClient } from '@angular/common/http';
import { inject, Service, signal } from '@angular/core';
import { IMember } from '../interfaces/members.interfaces';

@Service()
export class LikesService {
    private baseUrl = 'http://localhost:5001/api/';
    private http = inject(HttpClient);

    likeIds = signal<string[]>([]);

    toggleLike(targetMemberId: string) {
        return this.http.post(`${this.baseUrl}likes/${targetMemberId}`, {});
    }

    getLikes(predicate: string) {
        return this.http.get<IMember[]>(this.baseUrl + 'likes?predicate=' + predicate);
    }

    getLikeIds() {
        return this.http.get<string[]>(this.baseUrl + 'likes/list').subscribe({
            next: (ids) => this.likeIds.set(ids),
        });
    }
}
