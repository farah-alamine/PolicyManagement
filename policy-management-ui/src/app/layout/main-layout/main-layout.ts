import { Component, inject } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { Navbar } from '../navbar/navbar';
import { AuthenticationService } from '../../features/authentication/services/authentication.service';

@Component({
  selector: 'app-main-layout',
  standalone: true,
  imports: [Navbar, RouterOutlet],
  templateUrl: './main-layout.html',
  styleUrl: './main-layout.css'
})
export class MainLayout {
  readonly authenticationService = inject(AuthenticationService);
  private readonly router = inject(Router);

  logout(): void {
    this.authenticationService.logout();
    this.router.navigateByUrl('/login');
  }
}