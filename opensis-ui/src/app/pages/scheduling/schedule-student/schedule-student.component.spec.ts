import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ScheduleStudentComponent } from './schedule-student.component';

describe('ScheduleStudentComponent', () => {
  let component: ScheduleStudentComponent;
  let fixture: ComponentFixture<ScheduleStudentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ScheduleStudentComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ScheduleStudentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
