import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddMedicalComponent } from './add-medical.component';

describe('AddMedicalComponent', () => {
  let component: AddMedicalComponent;
  let fixture: ComponentFixture<AddMedicalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AddMedicalComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AddMedicalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
