import { Injectable, signal, computed } from '@angular/core';
import { Category, Product, ProductFilter } from '../../../shared/models/product.model';

@Injectable({ providedIn: 'root' })
export class ProductStore {
    // State
    private _products = signal<Product[]>([]);
    private _categories = signal<Category[]>([]);
    private _loading = signal(false);
    private _totalCount = signal(0);
    private _filter = signal<ProductFilter>({ page: 1, pageSize: 3 });

    // Selectors
    readonly products = this._products.asReadonly();
    readonly loading = this._loading.asReadonly();
    readonly totalCount = this._totalCount.asReadonly();
    readonly filter = this._filter.asReadonly();
    readonly totalPages = computed(() => Math.ceil(this._totalCount() / this._filter().pageSize));

    // Mutations
    setProducts(products: Product[], totalCount: number): void {
        this._products.set(products);
        this._totalCount.set(totalCount);
    }

    setLoading(loading: boolean): void {
        this._loading.set(loading);
    }

    updateFilter(partial: Partial<ProductFilter>): void {
        this._filter.update(f => ({ ...f, ...partial }));
    }

    resetFilter(): void {
        this._filter.set({ page: 1, pageSize: 3 });
    }
}
