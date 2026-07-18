import { Component, inject } from '@angular/core';
import {
  Router,
  RouterLink,
  RouterLinkActive
} from '@angular/router';

import { AuthenticationService } from '../../features/authentication/services/authentication.service';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [RouterLink, RouterLinkActive],
  templateUrl: './navbar.html',
  styleUrl: './navbar.css'
})
export class Navbar {
  readonly authenticationService = inject(AuthenticationService);
  private readonly router = inject(Router);

  logout(): void {
    this.authenticationService.logout();
    this.router.navigateByUrl('/login');
  }
}