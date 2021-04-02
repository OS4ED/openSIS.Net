import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReportCardCommentsComponent } from './report-card-comments.component';

describe('ReportCardCommentsComponent', () => {
  let component: ReportCardCommentsComponent;
  let fixture: ComponentFixture<ReportCardCommentsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ReportCardCommentsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ReportCardCommentsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
