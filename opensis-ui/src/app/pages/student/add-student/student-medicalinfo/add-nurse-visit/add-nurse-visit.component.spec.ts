import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddNurseVisitComponent } from './add-nurse-visit.component';

describe('AddNurseVisitComponent', () => {
  let component: AddNurseVisitComponent;
  let fixture: ComponentFixture<AddNurseVisitComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AddNurseVisitComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AddNurseVisitComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
