import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MissingAttendanceComponent } from './missing-attendance.component';

describe('MissingAttendanceComponent', () => {
  let component: MissingAttendanceComponent;
  let fixture: ComponentFixture<MissingAttendanceComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MissingAttendanceComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MissingAttendanceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
