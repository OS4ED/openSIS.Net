import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StudentReEnrollComponent } from './student-re-enroll.component';

describe('StudentReEnrollComponent', () => {
  let component: StudentReEnrollComponent;
  let fixture: ComponentFixture<StudentReEnrollComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ StudentReEnrollComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(StudentReEnrollComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
