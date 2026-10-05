import { Component, inject, OnInit, signal } from '@angular/core';
import { ProductService } from '../services/product.service';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { Product } from '../../../shared/models/product.model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-product-detail',
  imports: [CommonModule, RouterModule],
  templateUrl: './product-detail.html',
  styleUrl: './product-detail.scss',
})
export class ProductDetailComponent implements OnInit {
  private productService = inject(ProductService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  product = signal<Product | null>(null);
  isLoading = signal(false);
  error = signal<string | null>(null);

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (!id) { this.router.navigate(['/catalog/products']); return; }
    this.isLoading.set(true);
    this.productService.getById(id).subscribe({
      next: (prod) => {
        this.product.set(prod);
        this.isLoading.set(false);
      },
      error: () => {
        this.error.set('Product not found');
        this.isLoading.set(false);
      }
    })
  }

}
