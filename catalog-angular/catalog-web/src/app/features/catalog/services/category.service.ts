

import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { environment } from "../../../../environments/environment";
import { Observable } from "rxjs";
import { Category, CreateCategoryRequest, UpdateCategoryRequest } from "../../../shared/models/category.mode";

@Injectable({ providedIn: 'root' })
export class CategoryService {
    private http = inject(HttpClient);
    private apiUrl = `${environment.apiUrl}/categories`;

    getAll(isActive?: boolean): Observable<Category[]> {
        const params: Record<string, string> = {};
        if (isActive) params['isActive'] = String(isActive);
        return this.http.get<Category[]>(this.apiUrl, { params });
    }

    getById(id: string): Observable<Category> {
        return this.http.get<Category>(`${this.apiUrl}/${id}`);
    }

    create(request: CreateCategoryRequest): Observable<Category> {
        return this.http.post<Category>(this.apiUrl, request);
    }

    update(id: string, request: UpdateCategoryRequest): Observable<Category> {
        return this.http.put<Category>(`${this.apiUrl}/${id}`, request);
    }

    delete(id: string): Observable<void> {
        return this.http.delete<void>(`${this.apiUrl}/${id}`);
    }
}