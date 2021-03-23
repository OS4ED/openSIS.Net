import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StaffDataImportComponent } from './staff-data-import.component';

describe('StudentDataImportComponent', () => {
  let component: StaffDataImportComponent;
  let fixture: ComponentFixture<StaffDataImportComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ StaffDataImportComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(StaffDataImportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
