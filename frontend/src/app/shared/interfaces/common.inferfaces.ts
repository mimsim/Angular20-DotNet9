import { HttpResponse } from "@angular/common/http";

export interface CacheEntry {
    response: HttpResponse<unknown>;
    timestamp: number;
}