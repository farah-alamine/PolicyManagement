import {
  Component,
  DestroyRef,
  OnInit,
  inject,
  signal
} from '@angular/core';

import { DatePipe } from '@angular/common';
import {
  FormControl,
  ReactiveFormsModule
} from '@angular/forms';
import { RouterLink } from '@angular/router';

import {
  debounceTime,
  distinctUntilChanged
} from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

import { Policy } from '../../models/policy.model';
import { PolicyService } from '../../services/policy.service';
import { PageHeader } from '../../../../shared/components/page-header/page-header';

@Component({
  selector: 'app-policy-list',
  standalone: true,
  imports: [
    RouterLink,
    DatePipe,
    ReactiveFormsModule,
    PageHeader
  ],
  templateUrl: './policy-list.html',
  styleUrl: './policy-list.css'
})
export class PolicyList implements OnInit {
  private readonly policyService =
    inject(PolicyService);

  private readonly destroyRef =
    inject(DestroyRef);

  readonly policies = signal<Policy[]>([]);
  readonly isLoading = signal(false);
  readonly errorMessage = signal('');

  readonly deletingPolicyGuid =
    signal<string | null>(null);

  readonly pageNumber = signal(1);
  readonly pageSize = signal(10);
  readonly totalCount = signal(0);
  readonly totalPages = signal(0);
  readonly searchTerm = signal('');

  readonly searchControl =
    new FormControl('', {
      nonNullable: true
    });

  readonly pageSizeOptions = [
    5,
    10,
    20,
    50
  ];

  ngOnInit(): void {
    this.configureSearch();
    this.loadPolicies();
  }

  loadPolicies(): void {
    this.isLoading.set(true);
    this.errorMessage.set('');

    this.policyService
      .getAll(
        this.pageNumber(),
        this.pageSize(),
        this.searchTerm()
      )
      .subscribe({
        next: response => {
          this.policies.set(response.items);
          this.pageNumber.set(response.pageNumber);
          this.pageSize.set(response.pageSize);
          this.totalCount.set(response.totalCount);
          this.totalPages.set(response.totalPages);
          this.isLoading.set(false);
        },

        error: error => {
          console.error(
            'Failed to load policies',
            error
          );

          this.errorMessage.set(
            'Unable to load policies.'
          );

          this.isLoading.set(false);
        }
      });
  }

  goToPage(page: number): void {
    if (
      page < 1 ||
      page > this.totalPages() ||
      page === this.pageNumber() ||
      this.isLoading()
    ) {
      return;
    }

    this.pageNumber.set(page);
    this.loadPolicies();
  }

  previousPage(): void {
    this.goToPage(
      this.pageNumber() - 1
    );
  }

  nextPage(): void {
    this.goToPage(
      this.pageNumber() + 1
    );
  }

  changePageSize(event: Event): void {
    const selectElement =
      event.target as HTMLSelectElement;

    const newPageSize =
      Number(selectElement.value);

    this.pageSize.set(newPageSize);
    this.pageNumber.set(1);

    this.loadPolicies();
  }

  clearSearch(): void {
    this.searchControl.setValue('');
  }

  getVisiblePages(): number[] {
    const totalPages = this.totalPages();
    const currentPage = this.pageNumber();

    if (totalPages <= 5) {
      return Array.from(
        { length: totalPages },
        (_, index) => index + 1
      );
    }

    let startPage = Math.max(
      currentPage - 2,
      1
    );

    let endPage = Math.min(
      startPage + 4,
      totalPages
    );

    startPage = Math.max(
      endPage - 4,
      1
    );

    return Array.from(
      {
        length:
          endPage - startPage + 1
      },
      (_, index) =>
        startPage + index
    );
  }

  deletePolicy(policy: Policy): void {
    const confirmed = window.confirm(
      `Are you sure you want to delete "${policy.name}"?`
    );

    if (!confirmed) {
      return;
    }

    this.deletingPolicyGuid.set(
      policy.recordGuid
    );

    this.errorMessage.set('');

    this.policyService
      .delete(policy.recordGuid)
      .subscribe({
        next: () => {
          this.deletingPolicyGuid.set(null);

          /*
           * When the last item on a page is
           * deleted, move back one page.
           */
          if (
            this.policies().length === 1 &&
            this.pageNumber() > 1
          ) {
            this.pageNumber.update(
              currentPage =>
                currentPage - 1
            );
          }

          this.loadPolicies();
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

  private configureSearch(): void {
    this.searchControl.valueChanges
      .pipe(
        debounceTime(400),
        distinctUntilChanged(),
        takeUntilDestroyed(
          this.destroyRef
        )
      )
      .subscribe(value => {
        this.searchTerm.set(
          value.trim()
        );

        this.pageNumber.set(1);
        this.loadPolicies();
      });
  }
}