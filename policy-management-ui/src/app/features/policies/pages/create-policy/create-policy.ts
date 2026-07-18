import { Component, inject, signal } from '@angular/core';
import { PolicyForm } from '../../components/policy-form/policy-form';
import { PolicyTypeService } from '../../services/policy-type';
import { PolicyType } from '../../models/policy-type.model';
import { CreatePolicyRequest } from '../../models/create-policy-request.model';
import { Router } from '@angular/router';
import { PolicyService } from '../../services/policy.service';
@Component({
  selector: 'app-create-policy',
  standalone: true,
  imports: [PolicyForm],
  templateUrl: './create-policy.html',
  styleUrl: './create-policy.css'
})
export class CreatePolicy {
  private readonly policyTypeService = inject(PolicyTypeService);
  private readonly policyService = inject(PolicyService);
  private readonly router = inject(Router);
  readonly policyTypes = signal<PolicyType[]>([]);
  readonly isLoadingPolicyTypes = signal(false);
  readonly errorMessage = signal('');
  readonly isSubmitting = signal(false);
  readonly submitErrorMessage = signal('');
  constructor() {
    this.loadPolicyTypes();
  }

  private loadPolicyTypes(): void {
    this.isLoadingPolicyTypes.set(true);
    this.errorMessage.set('');

    this.policyTypeService.getAll().subscribe({
      next: policyTypes => {
        this.policyTypes.set(policyTypes);
        this.isLoadingPolicyTypes.set(false);
      },
      error: () => {
        this.errorMessage.set('Could not load policy types.');
        this.isLoadingPolicyTypes.set(false);
      }
    });
  }

createPolicy(request: CreatePolicyRequest): void {
  this.isSubmitting.set(true);
  this.submitErrorMessage.set('');

  this.policyService.create(request).subscribe({
    next: policy => {
      this.isSubmitting.set(false);

      this.router.navigate([
        '/policies',
        policy.recordGuid
      ]);
    },
    error: error => {
      console.error('Create policy failed:', error);

      this.submitErrorMessage.set(
        'The policy could not be created. Please try again.'
      );

      this.isSubmitting.set(false);
    }
  });
}
}