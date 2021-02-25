import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddVariableScheduleComponent } from './add-variable-schedule.component';

describe('AddVariableScheduleComponent', () => {
  let component: AddVariableScheduleComponent;
  let fixture: ComponentFixture<AddVariableScheduleComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AddVariableScheduleComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AddVariableScheduleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
