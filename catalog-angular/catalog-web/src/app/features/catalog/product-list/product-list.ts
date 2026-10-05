import { CommonModule } from '@angular/common';
import { Component, inject, OnInit, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { ProductService } from '../services/product.service';
import { CategoryService } from '../services/category.service';
import { ProductStore } from '../store/product.store';
import { AuthService } from '../../../core/services/auth.service';
import { Category } from '../../../shared/models/category.mode';
import { Product } from '../../../shared/models/product.model';

@Component({
  selector: 'app-product-list',
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './product-list.html',
  styleUrl: './product-list.scss',
})
export class ProductListComponent implements OnInit {
  private productService = inject(ProductService);
  private categoryService = inject(CategoryService);
  store = inject(ProductStore)
  auth = inject(AuthService);

  categories = signal<Category[]>([]);
  error = signal<string | null>(null);

  searchTerm = '';
  selectedCategoryId = '';
  selectedIsActive = '';

  ngOnInit(): void {
    this.loadCategories();
    this.loadProducts();
    console.log('isAdmin', this.auth.isAdmin());
  }

  loadCategories(): void {
    this.categoryService.getAll(true).subscribe({
      next: (cats) => this.categories.set(cats),
      error: (err) => this.error.set(err.message || 'Error al cargar las categorías'),
    });
  }

  loadProducts(): void {
    this.store.setLoading(true);
    this.error.set(null);
    const f = this.store.filter();
    this.productService.getAll(f).subscribe({
      next: res => {
        this.store.setProducts(res.items, res.totalCount);
        this.store.setLoading(false);
      },
      error: () => {
        this.error.set('Failed to load products');
        this.store.setLoading(false);
      }
    });
  }

  applyFilters(): void {
    this.store.updateFilter({
      page: 1,
      search: this.searchTerm || undefined,
      categoryId: this.selectedCategoryId || undefined,
      isActive: this.selectedIsActive === '' ? undefined : this.selectedIsActive === 'true'
    });
    this.loadProducts();
  }

  clearFilters(): void {
    this.searchTerm = '';
    this.selectedCategoryId = '';
    this.selectedIsActive = '';
    this.store.resetFilter();
    this.loadProducts();
  }

  goToPage(page: number): void {
    this.store.updateFilter({ page });
    this.loadProducts();
  }

  deleteProduct(product: Product): void {
    if (!confirm(`Are you sure you want to deactivate "${product.name}"?`)) return;
    this.productService.delete(product.id).subscribe({
      next: () => this.loadProducts(),
      error: () => this.error.set('Failed to delete product')
    });
  }

  getPages(): number[] {
    const total = this.store.totalPages();
    return Array.from({ length: total }, (_, i) => i + 1);
  }
}
