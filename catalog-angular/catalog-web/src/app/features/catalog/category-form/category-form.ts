import { CommonModule } from '@angular/common';
import { Component, inject, OnInit, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { CategoryService } from '../services/category.service';
import { ProductStore } from '../store/product.store';

@Component({
  selector: 'app-category-form',
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './category-form.html',
  styleUrl: './category-form.scss',
})
export class CategoryFormComponent implements OnInit {
  private fb = inject(FormBuilder);
  private categoryService = inject(CategoryService);
  private store = inject(ProductStore);
  private router = inject(Router);
  private route = inject(ActivatedRoute);

  isEditMode = false;
  categoryId: string | null = null;
  loading = signal(false);
  submitting = signal(false);
  error = signal<string | null>(null);
  form = this.fb.group({
    name: ['', [Validators.required, Validators.maxLength(200)]],
    description: [''],
  });

  ngOnInit(): void {
    this.categoryId = this.route.snapshot.paramMap.get('id');
    this.isEditMode = !!this.categoryId;
    if (this.isEditMode && this.categoryId) {
      this.loadCategory(this.categoryId);
    }
  }

  loadCategory(id: string): void {
    this.loading.set(true);
    this.categoryService.getById(id).subscribe({
      next: category => {
        this.form.patchValue({
          name: category.name,
          description: category.description
        });
        this.loading.set(false);
      },
      error: () => {
        this.error.set('Category not found');
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

    const request$ = this.isEditMode && this.categoryId
      ? this.categoryService.update(this.categoryId, value)
      : this.categoryService.create(value);

    request$.subscribe({
      next: () => this.router.navigate(['/catalog/categories']),
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
