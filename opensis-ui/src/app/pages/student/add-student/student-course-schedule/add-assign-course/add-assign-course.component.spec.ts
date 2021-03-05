import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddAssignCourseComponent } from './add-assign-course.component';

describe('AddAssignCourseComponent', () => {
  let component: AddAssignCourseComponent;
  let fixture: ComponentFixture<AddAssignCourseComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AddAssignCourseComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AddAssignCourseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
