import { TestBed } from '@angular/core/testing';

import { ReportCardService } from './report-card.service';

describe('ReportCardService', () => {
  let service: ReportCardService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ReportCardService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
