import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SuccessCopySchoolComponent } from './success-copy-school.component';

describe('SuccessCopySchoolComponent', () => {
  let component: SuccessCopySchoolComponent;
  let fixture: ComponentFixture<SuccessCopySchoolComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SuccessCopySchoolComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SuccessCopySchoolComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
