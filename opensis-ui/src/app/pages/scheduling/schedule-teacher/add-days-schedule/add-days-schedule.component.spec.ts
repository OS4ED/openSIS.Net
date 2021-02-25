import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddDaysScheduleComponent } from './add-days-schedule.component';

describe('AddDaysScheduleComponent', () => {
  let component: AddDaysScheduleComponent;
  let fixture: ComponentFixture<AddDaysScheduleComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AddDaysScheduleComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AddDaysScheduleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
