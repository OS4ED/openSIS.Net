import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FixedSchedulingComponent } from './fixed-scheduling.component';

describe('FixedSchedulingComponent', () => {
  let component: FixedSchedulingComponent;
  let fixture: ComponentFixture<FixedSchedulingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FixedSchedulingComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(FixedSchedulingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
