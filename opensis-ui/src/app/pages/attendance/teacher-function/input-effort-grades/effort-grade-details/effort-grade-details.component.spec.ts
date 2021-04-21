import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EffortGradeDetailsComponent } from './effort-grade-details.component';

describe('EffortGradeDetailsComponent', () => {
  let component: EffortGradeDetailsComponent;
  let fixture: ComponentFixture<EffortGradeDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EffortGradeDetailsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EffortGradeDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
