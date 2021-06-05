import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DeleteAssignmentsComponent } from './delete-assignments.component';

describe('DeleteAssignmentsComponent', () => {
  let component: DeleteAssignmentsComponent;
  let fixture: ComponentFixture<DeleteAssignmentsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DeleteAssignmentsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DeleteAssignmentsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
