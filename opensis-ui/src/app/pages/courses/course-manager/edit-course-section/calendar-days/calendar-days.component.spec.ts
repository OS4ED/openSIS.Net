import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CalendarDaysComponent } from './calendar-days.component';

describe('CalendarDaysComponent', () => {
  let component: CalendarDaysComponent;
  let fixture: ComponentFixture<CalendarDaysComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CalendarDaysComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CalendarDaysComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
