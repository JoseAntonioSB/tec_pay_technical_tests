import { Component, inject } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService } from './core/services/auth.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterModule, CommonModule],
  template: `
    <nav class="navbar">
      <a routerLink="/catalog/products" class="navbar-brand">Catalog</a>

      <div class="navbar-right">
        @if (auth.isLoggedIn()) {
          <div class="navbar-user">
            <span class="user-badge" [class.admin]="auth.isAdmin()">
              {{ auth.isAdmin() ? 'Admin' : 'User' }}
            </span>
            <span class="user-email">{{ auth.currentUser()?.email }}</span>
            <button class="btn-logout" (click)="auth.logout()">Sign out</button>
          </div>
        } @else {
          <a routerLink="/login" class="btn-login">Sign in</a>
        }
      </div>
    </nav>

    <main>
      <router-outlet />
    </main>
  `,
  styleUrl: './app.scss'
})
export class App {
  auth = inject(AuthService);
}
