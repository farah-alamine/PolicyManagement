import { Component, inject, signal } from '@angular/core';
import {
  FormBuilder,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';
import { Router } from '@angular/router';

import { AuthenticationService } from '../services/authentication.service';
import { LoginRequest } from '../models/login-request.model';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  private readonly formBuilder = inject(FormBuilder);
  private readonly authenticationService = inject(AuthenticationService);
  private readonly router = inject(Router);

  isSubmitting = signal(false);
  errorMessage = signal('');

  loginForm = this.formBuilder.nonNullable.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', Validators.required]
  });

  onSubmit(): void {
    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }

    this.isSubmitting.set(true);
    this.errorMessage.set('');

    const request: LoginRequest = this.loginForm.getRawValue();

    this.authenticationService.login(request).subscribe({
      next: () => {
        this.isSubmitting.set(false);
        this.router.navigateByUrl('/policies');
      },
      error: error => {
        this.isSubmitting.set(false);

        if (error.status === 401) {
          this.errorMessage.set('Invalid email or password.');
          return;
        }

        this.errorMessage.set(
          'An unexpected error occurred. Please try again.'
        );
      }
    });
  }
}