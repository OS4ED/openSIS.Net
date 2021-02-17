import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GroupDropComponent } from './group-drop.component';

describe('GroupDropComponent', () => {
  let component: GroupDropComponent;
  let fixture: ComponentFixture<GroupDropComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ GroupDropComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(GroupDropComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
