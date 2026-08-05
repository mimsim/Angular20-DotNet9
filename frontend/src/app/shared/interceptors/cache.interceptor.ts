// core/interceptors/cache.interceptor.ts
import { HttpInterceptorFn, HttpResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { of, tap } from 'rxjs';
import { HttpCacheService } from '../services/http-cache-service';

export const cacheInterceptor: HttpInterceptorFn = (req, next) => {
    const cacheService = inject(HttpCacheService);

    // Кешираме само GET заявки
    if (req.method !== 'GET') {
        return next(req);
    }

    // По желание: пропусни кеша за определени заявки
    if (req.headers.has('X-Skip-Cache')) {
        return next(req);
    }

    const cacheKey = req.urlWithParams;
    const cachedResponse = cacheService.get(cacheKey);

    if (cachedResponse) {
        return of(cachedResponse.clone());
    }

    return next(req).pipe(
        tap(event => {
            if (event instanceof HttpResponse) {
                cacheService.set(cacheKey, event);
            }
        })
    );
};