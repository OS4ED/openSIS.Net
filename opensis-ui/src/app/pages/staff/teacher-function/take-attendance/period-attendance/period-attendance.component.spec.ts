import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PeriodAttendanceComponent } from './period-attendance.component';

describe('PeriodAttendanceComponent', () => {
  let component: PeriodAttendanceComponent;
  let fixture: ComponentFixture<PeriodAttendanceComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PeriodAttendanceComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PeriodAttendanceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
