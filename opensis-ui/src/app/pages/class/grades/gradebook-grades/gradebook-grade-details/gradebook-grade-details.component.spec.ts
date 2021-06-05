import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GradebookGradeDetailsComponent } from './gradebook-grade-details.component';

describe('GradebookGradeDetailsComponent', () => {
  let component: GradebookGradeDetailsComponent;
  let fixture: ComponentFixture<GradebookGradeDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ GradebookGradeDetailsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(GradebookGradeDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
