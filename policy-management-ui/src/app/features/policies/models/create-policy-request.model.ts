export interface PolicyMemberRequest {
  firstName: string;
  lastName: string;
  dateOfBirth: string;
  relationshipToPolicyHolder: string;
}

export interface ClaimRequest {
  claimNumber: string;
  claimDate: string;
  amount: number;
  status: number;
  description: string | null;
}

export interface CreatePolicyRequest {
  name: string;
  description: string | null;
  effectiveDate: string;
  expiryDate: string;
  policyTypeGuid: string;
  members: PolicyMemberRequest[];
  claims: ClaimRequest[];
}