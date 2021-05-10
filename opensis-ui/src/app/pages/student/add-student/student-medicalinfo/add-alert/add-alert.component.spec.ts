import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddAlertComponent } from './add-alert.component';

describe('AddAlertComponent', () => {
  let component: AddAlertComponent;
  let fixture: ComponentFixture<AddAlertComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AddAlertComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AddAlertComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
