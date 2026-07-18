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
  readonly deletingPolicyGuid = signal<string | null>(null);
  ngOnInit(): void {
    this.loadPolicies();
  }

  loadPolicies(): void {
    this.isLoading.set(true);
    this.errorMessage.set('');

this.policyService.getAll(1, 10).subscribe({      next: response => {
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
  deletePolicy(policy: Policy): void {
  const confirmed = window.confirm(
    `Are you sure you want to delete "${policy.name}"?`
  );

  if (!confirmed) {
    return;
  }

  this.deletingPolicyGuid.set(policy.recordGuid);
  this.errorMessage.set('');

  this.policyService
    .delete(policy.recordGuid)
    .subscribe({
      next: () => {
        this.policies.update(currentPolicies =>
          currentPolicies.filter(
            currentPolicy =>
              currentPolicy.recordGuid
              !== policy.recordGuid
          )
        );

        this.deletingPolicyGuid.set(null);
      },
      error: error => {
        console.error(
          'Failed to delete policy',
          error
        );

        this.errorMessage.set(
          'The policy could not be deleted.'
        );

        this.deletingPolicyGuid.set(null);
      }
    });
}
}