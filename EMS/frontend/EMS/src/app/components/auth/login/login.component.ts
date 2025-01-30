import { Component, inject } from '@angular/core';
import { AuthService } from '../../../services/auth/auth.service';
import {
  FormControl,
  FormGroup,
  FormsModule,
  NgModel,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { SharedService } from '../../../services/shared/shared.service';
import { MatFormFieldModule } from '@angular/material/form-field';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  imports: [
    FormsModule,
    CommonModule,
    MatInputModule,
    MatButtonModule,
    MatFormFieldModule,

    ReactiveFormsModule,
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
})
export class LoginComponent {
  private _authService = inject(AuthService);
  private _sharedService = inject(SharedService);
  private router = inject(Router);

  username: string = '';
  password: string = '';

  token: string = '';
  refreshToken: string = '';

  constructor() {}

  login() {
    console.log('username ---> ', this.username);
    console.log('password --->', this.password);
    this._authService.login(this.username, this.password).subscribe({
      next: (response) => {
        if (response.token) {
          this.token = response.token;
          this.refreshToken = response.refreshToken;

          this._authService.setToken(this.token, this.refreshToken);
          this._sharedService.openSnackBar('User', 'login', true);
          this.router.navigate(['/employee']);
        } else {
          console.log('Login failed');
        }
      },
      error: (err) => {
        console.error('Login error', err);
      },
    });
  }
}
