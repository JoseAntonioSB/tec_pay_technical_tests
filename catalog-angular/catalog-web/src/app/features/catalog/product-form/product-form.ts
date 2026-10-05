import { Component, inject, OnInit, signal } from '@angular/core';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ProductService } from '../services/product.service';
import { Category } from '../../../shared/models/category.mode';
import { CategoryService } from '../services/category.service';

@Component({
  selector: 'app-product-form',
  standalone: true,
  imports: [CommonModule, RouterModule, ReactiveFormsModule],
  templateUrl: './product-form.html',
  styleUrl: './product-form.scss',
})
export class ProductFormComponent implements OnInit {

  private fb = inject(FormBuilder);
  private productService = inject(ProductService);
  private categoryService = inject(CategoryService);

  private route = inject(ActivatedRoute);
  private router = inject(Router);

  categories = signal<Category[]>([]);
  isEditMode = false;
  productId: string | null = null;
  loading = signal(false);
  submitting = signal(false);
  error = signal<string | null>(null);


  form = this.fb.group({
    name: ['', [Validators.required, Validators.maxLength(200)]],
    description: [''],
    price: [0, [Validators.required, Validators.min(0)]],
    stock: [0, [Validators.required, Validators.min(0)]],
    categoryId: ['', Validators.required]
  });

  ngOnInit(): void {
    this.productId = this.route.snapshot.paramMap.get('id');
    this.isEditMode = !!this.productId;
    this.loadCategories();
    if (this.isEditMode && this.productId) {
      this.loadProduct(this.productId);
    }
  }

  loadCategories(): void {
    this.categoryService.getAll(true).subscribe({
      next: cats => this.categories.set(cats)
    });
  }

  loadProduct(id: string): void {
    this.loading.set(true);
    this.productService.getById(id).subscribe({
      next: product => {
        this.form.patchValue({
          name: product.name,
          description: product.description || '',
          price: product.price,
          stock: product.stock,
          categoryId: product.categoryId
        });
        this.loading.set(false);
      },
      error: () => {
        this.error.set('Product not found');
        this.loading.set(false);
      }
    });
  }

  onSubmit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.submitting.set(true);
    this.error.set(null);
    const value = this.form.value as any;

    const request$ = this.isEditMode && this.productId
      ? this.productService.update(this.productId, value)
      : this.productService.create(value);

    request$.subscribe({
      next: () => this.router.navigate(['/catalog/products']),
      error: (err) => {
        this.error.set(err.error?.message || 'An error occurred');
        this.submitting.set(false);
      }
    });
  }

  hasError(field: string, error: string): boolean {
    const control = this.form.get(field);
    return !!(control?.touched && control?.hasError(error));
  }
}
