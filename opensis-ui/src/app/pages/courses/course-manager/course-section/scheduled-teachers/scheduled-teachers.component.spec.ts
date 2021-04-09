import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ScheduledTeachersComponent } from './scheduled-teachers.component';

describe('ScheduledTeachersComponent', () => {
  let component: ScheduledTeachersComponent;
  let fixture: ComponentFixture<ScheduledTeachersComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ScheduledTeachersComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ScheduledTeachersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
