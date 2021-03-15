import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StudentDataImportComponent } from './student-data-import.component';

describe('StudentDataImportComponent', () => {
  let component: StudentDataImportComponent;
  let fixture: ComponentFixture<StudentDataImportComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ StudentDataImportComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(StudentDataImportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
