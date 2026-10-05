import { HttpErrorResponse, HttpInterceptorFn } from "@angular/common/http";
import { inject } from "@angular/core";
import { Router } from "@angular/router";
import { StorageService } from "../services/storage.service";
import { catchError, throwError } from "rxjs";

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
    const router = inject(Router);
    const storage = inject(StorageService);

    return next(req).pipe(
        catchError((err: HttpErrorResponse) => {
            if (err.status == 401) {
                storage.removeToken();
                router.navigate(['/login']);
            }
            return throwError(() => err);
        })
    )
}   
