export interface Policy {
  recordGuid: string;
  name: string;
  description: string | null;
  effectiveDate: string;
  expiryDate: string; 
  policyTypeGuid: string;
  policyTypeName: string;
  createdDate: string;
  members: PolicyMember[];
  claims: PolicyClaim[];
}
export interface PolicyMember {
  recordGuid: string;
  firstName: string;
  lastName: string;
  dateOfBirth: string;
  relationshipToPolicyHolder: string;
}

export interface PolicyClaim {
  recordGuid: string;
  claimNumber: string;
  claimDate: string;
  amount: number;
  status: number;
  description: string | null;
}