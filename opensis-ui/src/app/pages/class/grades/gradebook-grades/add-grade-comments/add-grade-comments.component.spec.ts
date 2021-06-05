import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddGradeCommentsComponent } from './add-grade-comments.component';

describe('AddGradeCommentsComponent', () => {
  let component: AddGradeCommentsComponent;
  let fixture: ComponentFixture<AddGradeCommentsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AddGradeCommentsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AddGradeCommentsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
