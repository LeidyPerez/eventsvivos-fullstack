import { Component, ChangeDetectorRef } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-login',
  imports: [FormsModule],
  templateUrl: './login.html',
  styleUrl: './login.scss'
})
export class Login {
  credentials = { username: '', password: '' };
  error = '';

  constructor(
    private http: HttpClient,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {}

  login() {
    this.error = '';
    this.http.post<any>('http://localhost:5023/api/auth/login', this.credentials).subscribe({
      next: (data) => {
        localStorage.setItem('token', data.token);
        this.router.navigate(['/eventos']);
      },
      error: () => {
        this.error = 'Usuario o contraseña incorrectos';
        this.cdr.detectChanges();
      }
    });
  }
}