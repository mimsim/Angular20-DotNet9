import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Service } from '@angular/core';
import { IPaginatedResult, IMessage } from '../interfaces/messages.interface';

@Service()
export class MessageService {
    private http = inject(HttpClient);
    private baseUrl = 'http://localhost:5001/api/messages'; 

    getMessages(container: string, pageNumber = 1, pageSize = 10) {
        return this.http.get<IPaginatedResult<IMessage>>(this.baseUrl + {
            params: { container, pageNumber, pageSize },
        });
    }

    getMessageThread(recipientId: string) {
        return this.http.get<IMessage[]>(`${this.baseUrl}messages/thread/${recipientId}`);
    }

    sendMessage(recipientId: string, content: string) {
        return this.http.post<IMessage>(this.baseUrl + 'messages', { recipientId, content });
    }
}
