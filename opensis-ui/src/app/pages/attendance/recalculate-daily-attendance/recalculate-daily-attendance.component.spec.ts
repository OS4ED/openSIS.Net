import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RecalculateDailyAttendanceComponent } from './recalculate-daily-attendance.component';

describe('RecalculateDailyAttendanceComponent', () => {
  let component: RecalculateDailyAttendanceComponent;
  let fixture: ComponentFixture<RecalculateDailyAttendanceComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RecalculateDailyAttendanceComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RecalculateDailyAttendanceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
