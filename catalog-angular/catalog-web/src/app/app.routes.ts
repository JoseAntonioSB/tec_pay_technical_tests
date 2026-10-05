import { Routes } from '@angular/router';
import { adminGuard } from './core/guards/auth.guard';

export const routes: Routes = [
    { path: '', redirectTo: '/catalog/products', pathMatch: 'full' },

    { path: 'login', loadComponent: () => import('./features/auth/login/login').then(m => m.Login) },

    {
        path: 'catalog',
        children: [
            {
                path: 'products',
                loadComponent: () =>
                    import('./features/catalog/product-list/product-list').then(m => m.ProductListComponent)
            },
            {
                path: 'products/new',
                canActivate: [adminGuard],           //Admin
                loadComponent: () =>
                    import('./features/catalog/product-form/product-form').then(m => m.ProductFormComponent)
            },
            {
                path: 'products/:id',
                loadComponent: () =>
                    import('./features/catalog/product-detail/product-detail').then(m => m.ProductDetailComponent)
            },
            {
                path: 'products/:id/edit',
                canActivate: [adminGuard],           //Admin
                loadComponent: () =>
                    import('./features/catalog/product-form/product-form').then(m => m.ProductFormComponent)
            },
            {
                path: 'categories/new',
                canActivate: [adminGuard],           //Admin
                loadComponent: () =>
                    import('./features/catalog/category-form/category-form').then(m => m.CategoryFormComponent)
            }
        ]
    },

    { path: '**', redirectTo: '/catalog/products' }
];
