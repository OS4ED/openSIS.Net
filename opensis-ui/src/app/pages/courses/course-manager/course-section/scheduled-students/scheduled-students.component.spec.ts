import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ScheduledStudentsComponent } from './scheduled-students.component';

describe('ScheduledStudentsComponent', () => {
  let component: ScheduledStudentsComponent;
  let fixture: ComponentFixture<ScheduledStudentsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ScheduledStudentsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ScheduledStudentsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
