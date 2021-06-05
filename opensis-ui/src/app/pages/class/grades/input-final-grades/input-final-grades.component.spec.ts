import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InputFinalGradesComponent } from './input-final-grades.component';

describe('InputFinalGradesComponent', () => {
  let component: InputFinalGradesComponent;
  let fixture: ComponentFixture<InputFinalGradesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ InputFinalGradesComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(InputFinalGradesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
