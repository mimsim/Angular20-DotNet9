import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MemberCardComponent } from './member-card-component';

describe('MemberCardComponent', () => {
  let component: MemberCardComponent;
  let fixture: ComponentFixture<MemberCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MemberCardComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(MemberCardComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
