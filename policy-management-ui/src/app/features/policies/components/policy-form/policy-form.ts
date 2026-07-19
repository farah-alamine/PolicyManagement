import {
  Component,
  effect,
  inject,
  input,
  output
} from '@angular/core';

import {
  FormArray,
  FormBuilder,
  FormGroup,
  FormControl,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';

import { CreatePolicyRequest } from '../../models/create-policy-request.model';
import { Policy } from '../../models/policy.model';
import { PolicyType } from '../../models/policy-type.model';
import { dateRangeValidator } from '../../validators/date-range.validator';

type PolicyMemberForm = FormGroup<{
  firstName: FormControl<string>;
  lastName: FormControl<string>;
  dateOfBirth: FormControl<string>;
  relationshipToPolicyHolder: FormControl<string>;
}>;

type ClaimForm = FormGroup<{
  claimNumber: FormControl<string>;
  claimDate: FormControl<string>;
  amount: FormControl<number>;
  status: FormControl<number>;
  description: FormControl<string>;
}>;
@Component({
  selector: 'app-policy-form',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './policy-form.html',
  styleUrl: './policy-form.css'
})
export class PolicyForm {
  private readonly formBuilder = inject(FormBuilder);

  readonly policyTypes = input<PolicyType[]>([]);
  readonly initialValue = input<Policy | null>(null);
  readonly isSubmitting = input(false);

  readonly formSubmitted = output<CreatePolicyRequest>();

  readonly policyForm = this.formBuilder.nonNullable.group(
    {
      name: [
        '',
        [
          Validators.required,
          Validators.maxLength(200)
        ]
      ],

      description: [''],

      effectiveDate: [
        '',
        Validators.required
      ],

      expiryDate: [
        '',
        Validators.required
      ],

      policyTypeGuid: [
        '',
        Validators.required
      ],

    members: this.formBuilder.array<PolicyMemberForm>([]),
    claims: this.formBuilder.array<ClaimForm>([])
    },
    {
      validators: [dateRangeValidator]
    }
  );

  constructor() {
    effect(() => {
      const policy = this.initialValue();

      if (!policy) {
        return;
      }

      this.policyForm.patchValue({
        name: policy.name,
        description: policy.description ?? '',
        effectiveDate: policy.effectiveDate.substring(0, 10),
        expiryDate: policy.expiryDate.substring(0, 10),
        policyTypeGuid: policy.policyTypeGuid
      });

      this.members.clear();

      for (const member of policy.members ?? []) {
        this.members.push(
          this.createMemberForm({
            firstName: member.firstName,
            lastName: member.lastName,
            dateOfBirth: member.dateOfBirth.substring(0, 10),
            relationshipToPolicyHolder:
              member.relationshipToPolicyHolder
          })
        );
      }

      this.claims.clear();

      for (const claim of policy.claims ?? []) {
        this.claims.push(
          this.createClaimForm({
            claimNumber: claim.claimNumber,
            claimDate: claim.claimDate.substring(0, 10),
            amount: claim.amount,
            status: claim.status,
            description: claim.description ?? ''
          })
        );
      }
    });
  }

  get members(): FormArray<PolicyMemberForm> {
    return this.policyForm.controls.members;
  }

  get claims(): FormArray<ClaimForm> {
    return this.policyForm.controls.claims;
  }

  memberGroup(index: number): FormGroup {
    return this.members.at(index) as FormGroup;
  }

  claimGroup(index: number): FormGroup {
    return this.claims.at(index) as FormGroup;
  }

  addMember(): void {
    this.members.push(this.createMemberForm());
  }

  removeMember(index: number): void {
    this.members.removeAt(index);
  }

  addClaim(): void {
    this.claims.push(this.createClaimForm());
  }

  removeClaim(index: number): void {
    this.claims.removeAt(index);
  }

  submit(): void {
    if (this.isSubmitting()) {
      return;
    }

    if (this.policyForm.invalid) {
      this.policyForm.markAllAsTouched();
      return;
    }

    const formValue = this.policyForm.getRawValue();

    this.formSubmitted.emit({
      name: formValue.name.trim(),
      description:
        formValue.description.trim() || null,
      effectiveDate: formValue.effectiveDate,
      expiryDate: formValue.expiryDate,
      policyTypeGuid: formValue.policyTypeGuid,

      members: formValue.members.map(member => ({
        firstName: member.firstName.trim(),
        lastName: member.lastName.trim(),
        dateOfBirth: member.dateOfBirth,
        relationshipToPolicyHolder:
          member.relationshipToPolicyHolder.trim()
      })),

      claims: formValue.claims.map(claim => ({
        claimNumber: claim.claimNumber.trim(),
        claimDate: claim.claimDate,
        amount: Number(claim.amount),
        status: Number(claim.status),
        description:
          claim.description.trim() || null
      }))
    });
  }

 private createMemberForm(
  value?: {
    firstName: string;
    lastName: string;
    dateOfBirth: string;
    relationshipToPolicyHolder: string;
  }
): PolicyMemberForm {
  return this.formBuilder.nonNullable.group({
    firstName: [
      value?.firstName ?? '',
      Validators.required
    ],
    lastName: [
      value?.lastName ?? '',
      Validators.required
    ],
    dateOfBirth: [
      value?.dateOfBirth ?? '',
      Validators.required
    ],
    relationshipToPolicyHolder: [
      value?.relationshipToPolicyHolder ?? '',
      Validators.required
    ]
  });
}

private createClaimForm(
  value?: {
    claimNumber: string;
    claimDate: string;
    amount: number;
    status: number;
    description: string;
  }
): ClaimForm {
  return this.formBuilder.nonNullable.group({
    claimNumber: [
      value?.claimNumber ?? '',
      Validators.required
    ],
    claimDate: [
      value?.claimDate ?? '',
      Validators.required
    ],
    amount: [
      value?.amount ?? 0,
      [
        Validators.required,
        Validators.min(0.01)
      ]
    ],
    status: [
      value?.status ?? 1,
      Validators.required
    ],
    description: [
      value?.description ?? ''
    ]
  });
}
}