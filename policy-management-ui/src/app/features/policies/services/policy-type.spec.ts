import { TestBed } from '@angular/core/testing';

import { PolicyType } from './policy-type';

describe('PolicyType', () => {
  let service: PolicyType;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PolicyType);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
