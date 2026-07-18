import { Component, inject, OnInit, signal } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { PolicyForm } from '../../components/policy-form/policy-form';
import { PolicyService } from '../../services/policy.service';
import { PolicyTypeService } from '../../services/policy-type';
import { Policy } from '../../models/policy.model';
import { PolicyType } from '../../models/policy-type.model';
import { CreatePolicyRequest } from '../../models/create-policy-request.model';

@Component({
  selector: 'app-edit-policy',
  standalone: true,
  imports: [PolicyForm],
  templateUrl: './edit-policy.html',
  styleUrl: './edit-policy.css'
})
export class EditPolicy implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly policyService = inject(PolicyService);
  private readonly policyTypeService = inject(PolicyTypeService);

  readonly policy = signal<Policy | null>(null);
  readonly policyTypes = signal<PolicyType[]>([]);
  readonly isLoading = signal(false);
  readonly isSubmitting = signal(false);
  readonly errorMessage = signal('');

  private guid = '';

  ngOnInit(): void {
    const guid = this.route.snapshot.paramMap.get('guid');

    if (!guid) {
      this.errorMessage.set('Invalid policy identifier.');
      return;
    }

    this.guid = guid;

    this.loadPolicyTypes();
    this.loadPolicy();
  }

  private loadPolicyTypes(): void {
    this.policyTypeService.getAll().subscribe({
      next: policyTypes => {
        this.policyTypes.set(policyTypes);
      },
      error: error => {
        console.error('Failed to load policy types', error);
        this.errorMessage.set('Could not load policy types.');
      }
    });
  }

  private loadPolicy(): void {
    this.isLoading.set(true);

    this.policyService.getById(this.guid).subscribe({
      next: policy => {
        this.policy.set(policy);
        this.isLoading.set(false);
      },
      error: error => {
        console.error('Failed to load policy', error);
        this.errorMessage.set('The policy could not be loaded.');
        this.isLoading.set(false);
      }
    });
  }

  updatePolicy(request: CreatePolicyRequest): void {
    if (!this.guid) {
      return;
    }

    this.isSubmitting.set(true);
    this.errorMessage.set('');

 this.policyService.update(this.guid, request).subscribe({
  next: () => {
    this.isSubmitting.set(false);

    this.router.navigate([
      '/policies',
      this.guid
    ]);
  },
  error: error => {
    console.error('Failed to update policy', error);

    this.errorMessage.set(
      'The policy could not be updated.'
    );

    this.isSubmitting.set(false);
  }
});
  }
}