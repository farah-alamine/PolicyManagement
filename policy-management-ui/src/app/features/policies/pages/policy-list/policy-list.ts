import { CommonModule } from '@angular/common';
import { Component, OnInit, inject, signal } from '@angular/core';
import { Policy } from '../../models/policy.model';
import { PolicyService } from '../../services/policy.service';
import { DatePipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { PageHeader } from '../../../../shared/components/page-header/page-header';

@Component({
  selector: 'app-policy-list',
  standalone: true,
  imports: [RouterLink, DatePipe, PageHeader],
  templateUrl: './policy-list.html',
  styleUrl: './policy-list.css'
})
export class PolicyList implements OnInit {
  private readonly policyService = inject(PolicyService);

  readonly policies = signal<Policy[]>([]);
  readonly isLoading = signal(false);
  readonly errorMessage = signal('');

  ngOnInit(): void {
    this.loadPolicies();
  }

  loadPolicies(): void {
    this.isLoading.set(true);
    this.errorMessage.set('');

    this.policyService.getPolicies().subscribe({
      next: response => {
        this.policies.set(response.items);
        this.isLoading.set(false);
      },
      error: error => {
        console.error('Failed to load policies', error);
        this.errorMessage.set('Unable to load policies.');
        this.isLoading.set(false);
      }
    });
  }
}