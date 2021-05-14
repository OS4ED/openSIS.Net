import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InputEffortGradesComponent } from './input-effort-grades.component';

describe('InputEffortGradesComponent', () => {
  let component: InputEffortGradesComponent;
  let fixture: ComponentFixture<InputEffortGradesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ InputEffortGradesComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(InputEffortGradesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
