import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { MaterialModule } from '../../../material.module';
import { AuthService } from '../../services/auth-service';

@Component({
  selector: 'app-nav',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterModule,
    MaterialModule
  ],
  templateUrl: './nav-component.html',
  styleUrl: './nav-component.scss',
})
export class NavComponent {
  private readonly router = inject(Router);
  readonly authService = inject(AuthService);

  // Навигационни линкове (лесно разширими)
  readonly navLinks = [
    { label: 'Home', route: '/home' },
    { label: 'members', route: '/members' },
    // { label: 'Messages', route: '/messages' },
  ];

  // UI състояние на заявката
  readonly isLoading = signal(false);
  readonly errorMessage = signal<string | null>(null);
  onLogout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }


}