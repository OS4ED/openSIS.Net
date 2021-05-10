import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StudentTranscriptComponent } from './student-transcript.component';

describe('StudentTranscriptComponent', () => {
  let component: StudentTranscriptComponent;
  let fixture: ComponentFixture<StudentTranscriptComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ StudentTranscriptComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(StudentTranscriptComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
