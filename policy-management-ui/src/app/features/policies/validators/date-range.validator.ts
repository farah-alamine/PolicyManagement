import {
  AbstractControl,
  ValidationErrors,
  ValidatorFn
} from '@angular/forms';

export const dateRangeValidator: ValidatorFn = (
  control: AbstractControl
): ValidationErrors | null => {
  const effectiveDate = control.get('effectiveDate')?.value;
  const expiryDate = control.get('expiryDate')?.value;

  if (!effectiveDate || !expiryDate) {
    return null;
  }

  const effective = new Date(effectiveDate);
  const expiry = new Date(expiryDate);

  if (expiry <= effective) {
    return {
      invalidDateRange: true
    };
  }

  return null;
};