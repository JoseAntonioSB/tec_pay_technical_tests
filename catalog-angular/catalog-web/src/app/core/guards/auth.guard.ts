import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

export const authGuard: CanActivateFn = () => {
    const auth = inject(AuthService);
    const router = inject(Router);

    if (auth.isLoggedIn()) return true;

    return router.createUrlTree(['/login']);
};

export const adminGuard: CanActivateFn = () => {
    const auth = inject(AuthService);
    const router = inject(Router);

    if (auth.isAdmin()) return true;

    // Logged in but not admin → back to list
    if (auth.isLoggedIn()) return router.createUrlTree(['/catalog/products']);

    return router.createUrlTree(['/login']);
};
