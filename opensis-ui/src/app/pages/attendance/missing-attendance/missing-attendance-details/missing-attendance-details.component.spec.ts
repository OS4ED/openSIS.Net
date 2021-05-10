import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MissingAttendanceDetailsComponent } from './missing-attendance-details.component';

describe('MissingAttendanceDetailsComponent', () => {
  let component: MissingAttendanceDetailsComponent;
  let fixture: ComponentFixture<MissingAttendanceDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MissingAttendanceDetailsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MissingAttendanceDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
