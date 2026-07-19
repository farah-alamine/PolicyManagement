import {
  DatePipe,
  DecimalPipe
} from '@angular/common';

import {
  Component,
  inject,
  OnInit,
  signal
} from '@angular/core';

import {
  ActivatedRoute,
  RouterLink
} from '@angular/router';

import { Policy } from '../../models/policy.model';
import { PolicyService } from '../../services/policy.service';
import { PageHeader } from '../../../../shared/components/page-header/page-header';

@Component({
  selector: 'app-policy-details',
  standalone: true,
  imports: [
    DatePipe,
    DecimalPipe,
    RouterLink,
    PageHeader
  ],
  templateUrl: './policy-details.html',
  styleUrl: './policy-details.css'
})
export class PolicyDetails implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly policyService = inject(PolicyService);

  readonly policy = signal<Policy | null>(null);
  readonly isLoading = signal(false);
  readonly errorMessage = signal('');

  ngOnInit(): void {
    this.loadPolicy();
  }

  private loadPolicy(): void {
    const guid =
      this.route.snapshot.paramMap.get('guid');

    if (!guid) {
      this.errorMessage.set(
        'Invalid policy identifier.'
      );

      return;
    }

    this.isLoading.set(true);
    this.errorMessage.set('');

    this.policyService.getById(guid).subscribe({
      next: policy => {
        this.policy.set(policy);
        this.isLoading.set(false);
      },

      error: error => {
        console.error(
          'Failed to load policy',
          error
        );

        this.errorMessage.set(
          'The policy could not be loaded.'
        );

        this.isLoading.set(false);
      }
    });
  }
}