export interface Policy {
  recordGuid: string;
  name: string;
  description: string | null;
  effectiveDate: string;
  expiryDate: string;
  policyTypeName: string;
  createdDate: string;
}