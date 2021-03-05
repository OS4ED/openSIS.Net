import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StudentCourseScheduleComponent } from './student-course-schedule.component';

describe('StudentCourseScheduleComponent', () => {
  let component: StudentCourseScheduleComponent;
  let fixture: ComponentFixture<StudentCourseScheduleComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ StudentCourseScheduleComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(StudentCourseScheduleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
