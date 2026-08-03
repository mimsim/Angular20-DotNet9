import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
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
  private readonly fb = inject(FormBuilder);
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  // Навигационни линкове (лесно разширими)
  readonly navLinks = [
    { label: 'Home', route: '/home' },
    { label: 'members', route: '/members' },
    // { label: 'Messages', route: '/messages' },
  ];

  // Реактивна login форма
  readonly loginForm: FormGroup = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(6)]],
  });

  // UI състояние на заявката
  readonly isLoading = signal(false);
  readonly errorMessage = signal<string | null>(null);

  get emailControl() {
    return this.loginForm.get('email');
  }

  get passwordControl() {
    return this.loginForm.get('password');
  }

  onLogin(): void {
    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }

    this.isLoading.set(true);
    this.errorMessage.set(null);

    const { email, password } = this.loginForm.value;

    this.authService.login({ email, password }).subscribe({
      next: (response) => {
        this.isLoading.set(false);
        // TODO: запази token-a (напр. в AuthService/localStorage) и пренасочи
        console.log('Login успешен:', response);
        this.router.navigate(['/matches']);
      },
      error: (err) => {
        this.isLoading.set(false);
        this.errorMessage.set(
          err?.error?.message ?? 'Грешен email или парола. Опитай отново.'
        );
      },
    });
  }
}