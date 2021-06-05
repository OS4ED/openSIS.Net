import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GradebookGradesComponent } from './gradebook-grades.component';

describe('GradebookGradesComponent', () => {
  let component: GradebookGradesComponent;
  let fixture: ComponentFixture<GradebookGradesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ GradebookGradesComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(GradebookGradesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
