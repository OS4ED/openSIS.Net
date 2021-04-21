import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GradeDetailsComponent } from './grade-details.component';

describe('GradeDetailsComponent', () => {
  let component: GradeDetailsComponent;
  let fixture: ComponentFixture<GradeDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ GradeDetailsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(GradeDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
