import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import icClose from '@iconify/icons-ic/twotone-close';

@Component({
  selector: 'vex-add-assignment',
  templateUrl: './add-assignment.component.html',
  styleUrls: ['./add-assignment.component.scss']
})
export class AddAssignmentComponent implements OnInit {

  icClose = icClose;
  constructor(private dialogRef: MatDialogRef<AddAssignmentComponent>) { }

  ngOnInit(): void {
  }

}
