import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreatePolicy } from './create-policy';

describe('CreatePolicy', () => {
  let component: CreatePolicy;
  let fixture: ComponentFixture<CreatePolicy>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CreatePolicy],
    }).compileComponents();

    fixture = TestBed.createComponent(CreatePolicy);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
