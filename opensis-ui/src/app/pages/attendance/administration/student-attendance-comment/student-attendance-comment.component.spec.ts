import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StudentAttendanceCommentComponent } from './student-attendance-comment.component';

describe('StudentAttendanceCommentComponent', () => {
  let component: StudentAttendanceCommentComponent;
  let fixture: ComponentFixture<StudentAttendanceCommentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ StudentAttendanceCommentComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(StudentAttendanceCommentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
