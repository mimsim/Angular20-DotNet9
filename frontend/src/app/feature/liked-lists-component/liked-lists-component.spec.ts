import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LikedListsComponent } from './liked-lists-component';

describe('LikedListsComponent', () => {
  let component: LikedListsComponent;
  let fixture: ComponentFixture<LikedListsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LikedListsComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(LikedListsComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
