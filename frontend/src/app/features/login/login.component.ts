import { Component, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  ReactiveFormsModule,
  FormBuilder,
  FormGroup,
  Validators
} from '@angular/forms';
import { Router } from '@angular/router';
import { timeout, finalize } from 'rxjs/operators';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  loginForm: FormGroup;
  passwordVisible = false;
  isLoading = false;
  errorMessage: string | null = null;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required]
    });
  }

  togglePasswordVisibility(): void {
    this.passwordVisible = !this.passwordVisible;
  }

  onSubmit(): void {
    if (this.loginForm.valid && !this.isLoading) {
      this.isLoading = true;
      this.errorMessage = null;

      const email = this.loginForm.value.email;

      const credentials = {
        email: email,
        password: this.loginForm.value.password
      };

      this.authService.login(credentials).pipe(
        timeout(90000),
        finalize(() => {
          this.isLoading = false;
          this.cdr.detectChanges();
        })
      ).subscribe({
        next: (response) => {
          if (response.success) {
            console.log('Login successful via backend proxy.');

            this.authService.setAuthenticatedUser(
              response.companyCode || email
            );

            this.router.navigate(['/purchase-bill']);
          } else {
            this.errorMessage =
              response.message ||
              'Login failed. Please check your credentials.';

            this.cdr.detectChanges();
          }
        },

        error: (error) => {
          if (error.name === 'TimeoutError') {
            this.errorMessage =
              'The connection timed out. Please check your network and try again.';
          } else if (error.error && error.error.message) {
            this.errorMessage = error.error.message;
          } else {
            this.errorMessage =
              'An error occurred during login. Please try again.';
          }

          console.error('Login error', error);
          this.cdr.detectChanges();
        }
      });
    }
  }
}