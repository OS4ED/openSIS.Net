import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InputFinalGradeComponent } from './input-final-grade.component';

describe('InputFinalGradeComponent', () => {
  let component: InputFinalGradeComponent;
  let fixture: ComponentFixture<InputFinalGradeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ InputFinalGradeComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(InputFinalGradeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
