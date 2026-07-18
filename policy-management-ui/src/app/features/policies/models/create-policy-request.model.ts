export interface CreatePolicyRequest {
  name: string;
  description: string | null;
  effectiveDate: string;
  expiryDate: string;
  policyTypeGuid: string;
}