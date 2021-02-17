import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VariableSchedulingComponent } from './variable-scheduling.component';

describe('VariableSchedulingComponent', () => {
  let component: VariableSchedulingComponent;
  let fixture: ComponentFixture<VariableSchedulingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ VariableSchedulingComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(VariableSchedulingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
