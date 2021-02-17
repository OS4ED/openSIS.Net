import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RotatingSchedulingComponent } from './rotating-scheduling.component';

describe('RotatingSchedulingComponent', () => {
  let component: RotatingSchedulingComponent;
  let fixture: ComponentFixture<RotatingSchedulingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RotatingSchedulingComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RotatingSchedulingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
