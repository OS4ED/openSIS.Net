import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SearchParentComponent } from './search-parent.component';

describe('SearchParentComponent', () => {
  let component: SearchParentComponent;
  let fixture: ComponentFixture<SearchParentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SearchParentComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SearchParentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
