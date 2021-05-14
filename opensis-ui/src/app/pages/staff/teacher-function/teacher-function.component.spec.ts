import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TeacherFunctionComponent } from './teacher-function.component';

describe('TeacherFunctionComponent', () => {
  let component: TeacherFunctionComponent;
  let fixture: ComponentFixture<TeacherFunctionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TeacherFunctionComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TeacherFunctionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
