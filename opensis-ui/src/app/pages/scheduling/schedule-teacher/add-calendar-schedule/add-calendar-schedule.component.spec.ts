import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddCalendarScheduleComponent } from './add-calendar-schedule.component';

describe('AddCalendarScheduleComponent', () => {
  let component: AddCalendarScheduleComponent;
  let fixture: ComponentFixture<AddCalendarScheduleComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AddCalendarScheduleComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AddCalendarScheduleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
