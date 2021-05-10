import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddAbsencesComponent } from './add-absences.component';

describe('AddAbsencesComponent', () => {
  let component: AddAbsencesComponent;
  let fixture: ComponentFixture<AddAbsencesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AddAbsencesComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AddAbsencesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
