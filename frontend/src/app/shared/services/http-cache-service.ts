import { HttpResponse } from '@angular/common/http';
import { Service } from '@angular/core';
import { CacheEntry } from '../interfaces/common.inferfaces';


@Service()
export class HttpCacheService {
    private cache = new Map<string, CacheEntry>();
    private readonly maxAgeMs = 5 * 60 * 1000; // 5 минути

    get(key: string): HttpResponse<unknown> | null {
        const entry = this.cache.get(key);
        if (!entry) return null;

        const isExpired = Date.now() - entry.timestamp > this.maxAgeMs;
        if (isExpired) {
            this.cache.delete(key);
            return null;
        }

        return entry.response;
    }

    set(key: string, response: HttpResponse<unknown>): void {
        this.cache.set(key, { response, timestamp: Date.now() });
    }

    clear(key?: string): void {
        if (key) {
            this.cache.delete(key);
        } else {
            this.cache.clear();
        }
    }
}
