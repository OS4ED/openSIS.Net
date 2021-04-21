import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReportCardsComponent } from './report-cards.component';

describe('ReportCardsComponent', () => {
  let component: ReportCardsComponent;
  let fixture: ComponentFixture<ReportCardsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ReportCardsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ReportCardsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
