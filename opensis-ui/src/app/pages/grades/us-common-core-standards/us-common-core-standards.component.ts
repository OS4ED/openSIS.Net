import { Component, OnInit, Input } from '@angular/core';
import icMoreVert from '@iconify/icons-ic/twotone-more-vert';
import icAdd from '@iconify/icons-ic/baseline-add';
import icEdit from '@iconify/icons-ic/twotone-edit';
import icDelete from '@iconify/icons-ic/twotone-delete';
import icSearch from '@iconify/icons-ic/search';
import icFilterList from '@iconify/icons-ic/filter-list';
import icImport from '@iconify/icons-ic/twotone-unarchive';
import icUpdate from '@iconify/icons-ic/cached';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router} from '@angular/router';
import { fadeInUp400ms } from '../../../../@vex/animations/fade-in-up.animation';
import { stagger40ms } from '../../../../@vex/animations/stagger.animation';
import { TranslateService } from '@ngx-translate/core';
import { ViewDetailsComponent } from './view-details/view-details.component';
import { CryptoService } from 'src/app/services/Crypto.service';
import { RolePermissionListViewModel, RolePermissionViewModel } from 'src/app/models/roll-based-access.model';

@Component({
  selector: 'vex-us-common-core-standards',
  templateUrl: './us-common-core-standards.component.html',
  styleUrls: ['./us-common-core-standards.component.scss'],
  animations: [
    fadeInUp400ms,
    stagger40ms
  ]
})
export class UsCommonCoreStandardsComponent implements OnInit {
  @Input()
  columns = [
    { label: 'Standard Ref No', property: 'standard_ref_no', type: 'text', visible: true },
    { label: 'Subject', property: 'subject', type: 'text', visible: true },
    { label: 'Grade', property: 'grade', type: 'text', visible: true },
    { label: 'Course', property: 'course', type: 'text', visible: true },
    { label: 'Domain', property: 'domain', type: 'text', visible: true },
    { label: 'Topic', property: 'topic', type: 'text', visible: true },
    { label: 'Standard Details', property: 'standard_details', type: 'text', visible: false },
    { label: 'Actions', property: 'actions', type: 'text', visible: true }
  ];

  CommonCoreStandardsModelList;

  icMoreVert = icMoreVert;
  icAdd = icAdd;
  icEdit = icEdit;
  icDelete = icDelete;
  icSearch = icSearch;
  icImport = icImport;
  icFilterList = icFilterList;
  icUpdate = icUpdate;
  selectedOption = '1';
  loading:Boolean;
  editPermission = false;
  deletePermission = false;
  addPermission = false;
  permissionListViewModel: RolePermissionListViewModel = new RolePermissionListViewModel();
  permissionGroup: RolePermissionViewModel = new RolePermissionViewModel();

  constructor(private router: Router,private dialog: MatDialog,public translateService:TranslateService,
    private cryptoService: CryptoService) {
    translateService.use('en');
    this.CommonCoreStandardsModelList = [
      {subject: 'Mathematics', grade: 'Kindergarten', course: 'General Maths', domain: 'Counting and Cardinality', topic: 'Know number names and the count sequence.', standard_ref_no: 'CCSS.Math.Content.K.CC.A.1', standard_details: 'Count to 100 by ones and by tens.'},
      {subject: 'Mathematics', grade: 'Kindergarten', course: 'General Maths', domain: 'Counting and Cardinality', topic: 'Know number names and the count sequence.', standard_ref_no: 'CCSS.Math.Content.K.CC.A.2', standard_details: 'Count forward beginning from a given number within the known sequence (instead of having to begin at 1).'},
      {subject: 'Mathematics', grade: 'Kindergarten', course: 'General Maths', domain: 'Counting and Cardinality', topic: 'Know number names and the count sequence.', standard_ref_no: 'CCSS.Math.Content.K.CC.A.3', standard_details: 'Write numbers from 0 to 20. Represent a number of objects with a written numeral 0-20 (with 0 representing a count of no objects).'},
      {subject: 'Mathematics', grade: 'Kindergarten', course: 'General Maths', domain: 'Counting and Cardinality', topic: 'Count to tell the number of objects. ', standard_ref_no: 'CCSS.Math.Content.K.CC.B.4', standard_details: 'Understand the relationship between numbers and quantities; connect counting to cardinality.'},
      {subject: 'Mathematics', grade: 'Kindergarten', course: 'General Maths', domain: 'Counting and Cardinality', topic: 'Count to tell the number of objects.', standard_ref_no: 'CCSS.Math.Content.K.CC.B.4a', standard_details: 'When counting objects, say the number names in the standard order, pairing each object with one and only one number name and each number name with one and only one object.'},
      {subject: 'Mathematics', grade: 'Kindergarten', course: 'General Maths', domain: 'Counting and Cardinality', topic: 'Count to tell the number of objects.', standard_ref_no: 'CCSS.Math.Content.K.CC.B.4b', standard_details: 'Understand that the last number name said tells the number of objects counted. The number of objects is the same regardless of their arrangement or the order in which they were counted.'},
      {subject: 'Mathematics', grade: 'Kindergarten', course: 'General Maths', domain: 'Counting and Cardinality', topic: 'Count to tell the number of objects.', standard_ref_no: 'CCSS.Math.Content.K.CC.B.4c', standard_details: 'Understand that each successive number name refers to a quantity that is one larger.'},
      {subject: 'Mathematics', grade: 'Kindergarten', course: 'General Maths', domain: 'Counting and Cardinality', topic: 'Count to tell the number of objects.', standard_ref_no: 'CCSS.Math.Content.K.CC.B.5', standard_details: 'Count to answer “how many?” questions about as many as 20 things arranged in a line, a rectangular array, or a circle, or as many as 10 things in a scattered configuration; given a number from 1–20, count out that many objects.'},
      {subject: 'Mathematics', grade: 'Kindergarten', course: 'General Maths', domain: 'Counting and Cardinality', topic: 'Compare numbers.', standard_ref_no: 'CCSS.Math.Content.K.CC.B.6', standard_details: 'Identify whether the number of objects in one group is greater than, less than, or equal to the number of objects in another group, e.g., by using matching and counting strategies.1'},
      {subject: 'Mathematics', grade: 'Kindergarten', course: 'General Maths', domain: 'Counting and Cardinality', topic: 'Compare numbers.', standard_ref_no: 'CCSS.Math.Content.K.CC.B.7', standard_details: 'Compare two numbers between 1 and 10 presented as written numerals. '},
      {subject: 'Mathematics', grade: 'Kindergarten', course: 'General Maths', domain: 'Counting and Cardinality', topic: 'Understand addition, and understand subtraction.', standard_ref_no: 'CCSS.Math.Content.K.OA.A.1', standard_details: 'Represent addition and subtraction with objects, fingers, mental images, drawings1, sounds (e.g., claps), acting out situations, verbal explanations, expressions, or equations.'},
      {subject: 'Mathematics', grade: 'Kindergarten', course: 'General Maths', domain: 'Counting and Cardinality', topic: 'Understand addition, and understand subtraction.', standard_ref_no: 'CCSS.Math.Content.K.OA.A.2', standard_details: 'Solve addition and subtraction word problems, and add and subtract within 10, e.g., by using objects or drawings to represent the problem.'},
    ]
  }

  ngOnInit(): void {
    this.permissionListViewModel = JSON.parse(this.cryptoService.dataDecrypt(localStorage.getItem('permissions')));
    this.permissionGroup = this.permissionListViewModel?.permissionList.find(x => x.permissionGroup.permissionGroupId === 12);
    const permissionCategory = this.permissionGroup.permissionGroup.permissionCategory.find(x => x.permissionCategoryId === 26);
    const permissionSubCategory = permissionCategory.permissionSubcategory.find( x => x.permissionSubcategoryId === 28);
    this.editPermission = permissionSubCategory.rolePermission[0].canEdit;
    this.deletePermission = permissionSubCategory.rolePermission[0].canDelete;
    this.addPermission = permissionSubCategory.rolePermission[0].canAdd;
  }


  // goToAdd(){
  //   this.dialog.open(EditEthnicityComponent, {
  //     width: '500px'
  //   })
  // }

  openViewDetails() {
    this.dialog.open(ViewDetailsComponent, {
      width: '600px'
    });
  }

  toggleColumnVisibility(column, event) {
    event.stopPropagation();
    event.stopImmediatePropagation();
    column.visible = !column.visible;
  }

  get visibleColumns() {
    return this.columns.filter(column => column.visible).map(column => column.property);
  }


}
