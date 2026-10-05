import { HttpClient, HttpParams } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { environment } from "../../../../environments/environment";
import { CreateProductRequest, Product, ProductFilter, UpdateProductRequest } from "../../../shared/models/product.model";
import { PagedResult } from "../../../shared/models/paginated-response.model";
import { Observable } from "rxjs";


@Injectable({ providedIn: 'root' })
export class ProductService {
    private http = inject(HttpClient);
    private apiUrl = `${environment.apiUrl}/products`;

    getAll(filter: ProductFilter): Observable<PagedResult<Product>> {
        let params = new HttpParams()
            .set('page', filter.page)
            .set('pageSize', filter.pageSize);

        if (filter.search) {
            params = params.set('search', filter.search);
        }
        if (filter.categoryId) {
            params = params.set('categoryId', filter.categoryId);
        }
        if (filter.isActive !== undefined) {
            params = params.set('isActive', filter.isActive.toString());
        }
        return this.http.get<PagedResult<Product>>(this.apiUrl, { params });
    }

    getById(id: string): Observable<Product> {
        return this.http.get<Product>(`${this.apiUrl}/${id}`);
    }

    create(request: CreateProductRequest): Observable<Product> {
        return this.http.post<Product>(this.apiUrl, request);
    }

    update(id: string, request: UpdateProductRequest): Observable<Product> {
        return this.http.put<Product>(`${this.apiUrl}/${id}`, request);
    }

    delete(id: string): Observable<void> {
        return this.http.delete<void>(`${this.apiUrl}/${id}`);
    }
}