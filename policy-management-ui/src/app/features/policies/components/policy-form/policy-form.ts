import { Component, inject, input, output } from '@angular/core';
import {
  FormBuilder,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';
import { CreatePolicyRequest } from '../../models/create-policy-request.model';
import { PolicyType } from '../../models/policy-type.model';
import { dateRangeValidator } from '../../validators/date-range.validator';
import { effect } from '@angular/core';
import { Policy } from '../../models/policy.model';

@Component({
  selector: 'app-policy-form',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './policy-form.html',
  styleUrl: './policy-form.css'
})

export class PolicyForm {
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
  });
}
  private readonly formBuilder = inject(FormBuilder);

  readonly policyTypes = input<PolicyType[]>([]);
  readonly formSubmitted = output<CreatePolicyRequest>();
  readonly isSubmitting = input(false);
  readonly initialValue = input<Policy | null>(null);
  readonly policyForm = this.formBuilder.nonNullable.group(
  {
    name: ['', [Validators.required, Validators.maxLength(200)]],
    description: [''],
    effectiveDate: ['', Validators.required],
    expiryDate: ['', Validators.required],
    policyTypeGuid: ['', Validators.required]
  },
  {
    validators: [dateRangeValidator]
  }
);

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
    name: formValue.name,
    description: formValue.description || null,
    effectiveDate: formValue.effectiveDate,
    expiryDate: formValue.expiryDate,
    policyTypeGuid: formValue.policyTypeGuid
  });
}
}