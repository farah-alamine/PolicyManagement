import { Component, inject, output } from '@angular/core';
import {
  FormBuilder,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';
import { CreatePolicyRequest } from '../../models/create-policy-request.model';

@Component({
  selector: 'app-policy-form',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './policy-form.html',
  styleUrl: './policy-form.css'
})
export class PolicyForm {
  readonly formSubmitted = output<CreatePolicyRequest>();

private readonly formBuilder = inject(FormBuilder);

  readonly policyForm = this.formBuilder.nonNullable.group({
    name: ['', [Validators.required, Validators.maxLength(200)]],
    description: [''],
    effectiveDate: ['', Validators.required],
    expiryDate: ['', Validators.required],
    policyTypeGuid: ['', Validators.required]
  });

  submit(): void {
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