import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import icEdit from '@iconify/icons-ic/twotone-edit';
import icDelete from '@iconify/icons-ic/twotone-delete';
import icAdd from '@iconify/icons-ic/baseline-add';
import { FormGroup } from '@angular/forms';
import icSearch from '@iconify/icons-ic/search';
import icFilterList from '@iconify/icons-ic/filter-list';
import { MatDialog } from '@angular/material/dialog';
import { AddGradeComponent } from './add-grade/add-grade.component';

@Component({
  selector: 'vex-standard-grades',
  templateUrl: './standard-grades.component.html',
  styleUrls: ['./standard-grades.component.scss']
})
export class StandardGradesComponent implements OnInit {

  icEdit = icEdit;
  icDelete = icDelete;
  icAdd = icAdd;
  effortCategoryTitle: string;
  addCategory = false;
  form:FormGroup;
  buttonType: string;
  icSearch = icSearch;
  icFilterList = icFilterList;

  constructor(public translateService:TranslateService, private dialog: MatDialog) {
    translateService.use('en');
   }

  ngOnInit(): void {
  }

  goToAddCategory() {
    
    this.effortCategoryTitle="addNewGradeScale";
    this.addCategory = true;
    this.buttonType="submit";
  }
  closeAddCategory() {
    this.addCategory = false;
  }
  addGrade(){
    this.dialog.open(AddGradeComponent, {
      width: '500px'
    })
  }

}
