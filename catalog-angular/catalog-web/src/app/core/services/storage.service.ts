import { Injectable } from "@angular/core";

@Injectable({ providedIn: 'root' })
export class StorageService {
    getToken() {
        return localStorage.getItem('auth_token');
    }

    setToken(token: string) {
        localStorage.setItem('auth_token', token);
    }

    removeToken() {
        localStorage.removeItem('auth_token');
    }

    isAuthenticated(): boolean {
        return !!this.getToken();
    }

}