import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddTeacherCommentsComponent } from './add-teacher-comments.component';

describe('AddTeacherCommentsComponent', () => {
  let component: AddTeacherCommentsComponent;
  let fixture: ComponentFixture<AddTeacherCommentsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AddTeacherCommentsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AddTeacherCommentsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
