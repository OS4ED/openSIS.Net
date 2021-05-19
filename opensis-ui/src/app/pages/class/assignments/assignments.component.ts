import { Component, OnInit } from '@angular/core';
import icEdit from '@iconify/icons-ic/twotone-edit';
import icDeleteForever from '@iconify/icons-ic/twotone-delete-forever';
import icDelete from '@iconify/icons-ic/twotone-delete';
import { MatDialog } from '@angular/material/dialog';
import { AddAssignmentComponent } from './add-assignment/add-assignment.component';
import { CreateAssignmentComponent } from './create-assignment/create-assignment.component';

@Component({
  selector: 'vex-assignments',
  templateUrl: './assignments.component.html',
  styleUrls: ['./assignments.component.scss']
})
export class AssignmentsComponent implements OnInit {

  icEdit = icEdit;
  icDeleteForever = icDeleteForever;
  icDelete = icDelete;

  constructor(private dialog: MatDialog) {}

  ngOnInit(): void {
  }

  addAssignment(){
    this.dialog.open(AddAssignmentComponent, {
      width: '500px'
    });
  }

  createAssignment(){
    this.dialog.open(CreateAssignmentComponent, {
      width: '800px'
    });
  }

}
