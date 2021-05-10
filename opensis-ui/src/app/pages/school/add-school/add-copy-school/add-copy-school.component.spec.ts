import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddCopySchoolComponent } from './add-copy-school.component';

describe('AddCopySchoolComponent', () => {
  let component: AddCopySchoolComponent;
  let fixture: ComponentFixture<AddCopySchoolComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AddCopySchoolComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AddCopySchoolComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
