import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { AuthService, LoginRequest } from '../../../core/services/auth.service';

@Component({
  selector: 'app-login',
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './login.html',
  styleUrl: './login.scss',
})
export class Login {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private router = inject(Router);

  isSubmitting = signal(false);
  error = signal<string | null>(null);

  form = this.fb.group({
    email: ['admin@catalog.com', [Validators.required, Validators.email]],
    password: ['Admin123!', [Validators.required]]
  })

  onSubmit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    this.isSubmitting.set(true);
    this.error.set(null);

    const { email, password } = this.form.value as { email: string; password: string };

    this.authService.login({ email, password }).subscribe({
      next: () => {
        this.router.navigate(['/catalog/products']);
      },
      error: (err) => {
        console.log(err);
        this.error.set(err.status === 401 ? 'Invalid email or password.' : 'Login failed. Please try again.');
        this.isSubmitting.set(false);
      }
    })
  }

  hasError(field: string, error: string): boolean {
    const ctrl = this.form.get(field);
    return !!(ctrl?.touched && ctrl?.errors?.[error]);
  }
}
