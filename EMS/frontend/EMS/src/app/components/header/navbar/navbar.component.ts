import { Component, inject } from '@angular/core';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { SharedService } from '../../../services/shared/shared.service';
import { AuthService } from '../../../services/auth/auth.service';

@Component({
  selector: 'app-navbar',
  imports: [RouterLink, RouterLinkActive],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css',
})
export class NavbarComponent {
  private _sharedService = inject(SharedService);
  private _authService = inject(AuthService);
  private router = inject(Router);
  logOut() {
    this._authService.resetToken();
    this._sharedService.openSnackBar('User', 'logout', true);

    this.router.navigate(['/login']);
  }
}
