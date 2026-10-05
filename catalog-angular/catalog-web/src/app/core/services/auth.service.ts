import { HttpClient } from "@angular/common/http";
import { computed, inject, Injectable, signal } from "@angular/core";
import { Router } from "@angular/router";
import { BehaviorSubject, Observable, takeUntil, ReplaySubject, tap } from "rxjs";
import { StorageService } from "./storage.service";
import { environment } from '../../../environments/environment';


export interface LoginRequest {
    email: string
    password: string
}

export interface TokenResponse {
    token: string;
    email: string;
    role: string;
    expiresAt: string;
}

export interface CurrentUser {
    email: string;
    role: string;
}

@Injectable({ providedIn: 'root' })
export class AuthService {
    private http = inject(HttpClient);
    private router = inject(Router);
    private storage = inject(StorageService);

    private _currentUser = signal<CurrentUser | null>(this.getUserFromStorage());

    readonly currentUser = this._currentUser.asReadonly();
    readonly isLoggedIn = computed(() => !!this._currentUser());
    readonly isAdmin = computed(() => this.isLoggedIn() && this.currentUser()?.role === 'Admin');

    login(request: LoginRequest): Observable<TokenResponse> {
        console.log(`${environment.apiUrl}/auth/login`, request)
        return this.http
            .post<TokenResponse>(`${environment.apiUrl}/auth/login`, request)
            .pipe(
                tap(res => {
                    this.storage.setToken(res.token);
                    localStorage.setItem('current_user', JSON.stringify({ email: res.email, role: res.role }));
                    this._currentUser.set({ email: res.email, role: res.role });
                })
            );
    }

    logout(): void {
        this.storage.removeToken();
        localStorage.removeItem('current_user');
        this._currentUser.set(null);
        this.router.navigate(['/login']);
    }
    private getUserFromStorage(): CurrentUser | null {
        try {
            const raw = localStorage.getItem('current_user');
            return raw ? JSON.parse(raw) : null;
        } catch {
            return null;
        }
    }



}