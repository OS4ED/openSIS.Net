import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TeacherReassignmentComponent } from './teacher-reassignment.component';

describe('TeacherReassignmentComponent', () => {
  let component: TeacherReassignmentComponent;
  let fixture: ComponentFixture<TeacherReassignmentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TeacherReassignmentComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TeacherReassignmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
