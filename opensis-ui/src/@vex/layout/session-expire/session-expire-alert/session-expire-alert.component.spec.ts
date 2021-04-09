import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SessionExpireAlertComponent } from './session-expire-alert.component';

describe('SessionExpireAlertComponent', () => {
  let component: SessionExpireAlertComponent;
  let fixture: ComponentFixture<SessionExpireAlertComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SessionExpireAlertComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SessionExpireAlertComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
