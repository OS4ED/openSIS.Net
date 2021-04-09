import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StandardGradesComponent } from './standard-grades.component';

describe('StandardGradesComponent', () => {
  let component: StandardGradesComponent;
  let fixture: ComponentFixture<StandardGradesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ StandardGradesComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(StandardGradesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
