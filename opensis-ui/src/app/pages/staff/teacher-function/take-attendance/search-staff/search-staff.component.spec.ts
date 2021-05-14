import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SearchStaffComponent } from './search-staff.component';

describe('SearchStaffComponent', () => {
  let component: SearchStaffComponent;
  let fixture: ComponentFixture<SearchStaffComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SearchStaffComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SearchStaffComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
