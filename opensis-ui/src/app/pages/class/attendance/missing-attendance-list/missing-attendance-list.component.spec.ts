import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MissingAttendanceListComponent } from './missing-attendance-list.component';

describe('MissingAttendanceListComponent', () => {
  let component: MissingAttendanceListComponent;
  let fixture: ComponentFixture<MissingAttendanceListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MissingAttendanceListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MissingAttendanceListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
