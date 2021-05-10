import {
  Component,
  EventEmitter,
  Input,
  OnDestroy,
  OnInit,
  Output,
  ViewChild,
} from "@angular/core";
import { FormBuilder, NgForm } from "@angular/forms";
import { fadeInUp400ms } from "../../../../../@vex/animations/fade-in-up.animation";
import { stagger60ms } from "../../../../../@vex/animations/stagger.animation";
import { fadeInRight400ms } from "../../../../../@vex/animations/fade-in-right.animation";
import { TranslateService } from "@ngx-translate/core";
import icEdit from "@iconify/icons-ic/twotone-edit";
import icDelete from "@iconify/icons-ic/twotone-delete";
import icAdd from "@iconify/icons-ic/baseline-add";
import icComment from "@iconify/icons-ic/twotone-comment";
import icDeleteForever from "@iconify/icons-ic/twotone-delete-forever";
import { SchoolCreate } from "../../../../enums/school-create.enum";
import { StudentAddModel } from "../../../../models/student.model";
import { StudentService } from "../../../../services/student.service";
import { MatSnackBar } from "@angular/material/snack-bar";
import { ViewParentInfoModel } from "../../../../models/parent-info.model";
import { ParentInfoService } from "../../../../services/parent-info.service";
import { ImageCropperService } from "../../../../services/image-cropper.service";
import { ModuleIdentifier } from "../../../../enums/module-identifier.enum";
import {
  RolePermissionListViewModel,
  RolePermissionViewModel,
} from "../../../../models/roll-based-access.model";
import { CryptoService } from "../../../../services/Crypto.service";
import { DefaultValuesService } from "../../../../common/default-values.service";
import { MatDialog } from "@angular/material/dialog";
import { AddAlertComponent } from "./add-alert/add-alert.component";
import { AddMedicalComponent } from "./add-medical/add-medical.component";
import { AddImmunizationComponent } from "./add-immunization/add-immunization.component";
import { AddNurseVisitComponent } from "./add-nurse-visit/add-nurse-visit.component";
import { MatTabChangeEvent } from "@angular/material/tabs";
@Component({
  selector: "vex-student-medicalinfo",
  templateUrl: "./student-medicalinfo.component.html",
  styleUrls: ["./student-medicalinfo.component.scss"],
  animations: [stagger60ms, fadeInUp400ms, fadeInRight400ms],
})
export class StudentMedicalinfoComponent implements OnInit, OnDestroy {
  moduleIdentifier = ModuleIdentifier;
  studentCreate = SchoolCreate;
  @Input() studentCreateMode: SchoolCreate;
  @Input() categoryId;
  @Input() studentDetailsForViewAndEdit;
  @ViewChild("f") currentForm: NgForm;
  @Output() studentDetailsForParent = new EventEmitter<StudentAddModel>();
  studentAddModel: StudentAddModel = new StudentAddModel();
  parentInfoModel: ViewParentInfoModel = new ViewParentInfoModel();
  icEdit = icEdit;
  icDelete = icDelete;
  icDeleteForever = icDeleteForever;
  module = "Student";
  icAdd = icAdd;
  icComment = icComment;
  parentsFullName = [];
  cloneStudentAddModel;
  editPermission = false;
  deletePermission = false;
  addPermission = false;
  permissionListViewModel: RolePermissionListViewModel = new RolePermissionListViewModel();
  permissionGroup: RolePermissionViewModel = new RolePermissionViewModel();
  currentTab: string;

  constructor(
    private fb: FormBuilder,
    public translateService: TranslateService,
    private studentService: StudentService,
    private snackbar: MatSnackBar,
    private defaultValuesService: DefaultValuesService,
    private parentInfoService: ParentInfoService,
    private imageCropperService: ImageCropperService,
    private cryptoService: CryptoService,
    private dialog: MatDialog
  ) {
    translateService.use("en");
  }

  ngOnInit(): void {
    this.currentTab = 'activities';
    if (this.studentCreateMode === this.studentCreate.VIEW) {
      this.permissionListViewModel = JSON.parse(
        this.cryptoService.dataDecrypt(localStorage.getItem("permissions"))
      );
      this.permissionGroup = this.permissionListViewModel?.permissionList.find(
        (x) => x.permissionGroup.permissionGroupId === 3
      );
      const permissionCategory = this.permissionGroup.permissionGroup.permissionCategory.find(
        (x) => x.permissionCategoryId === 5
      );
      const permissionSubCategory = permissionCategory.permissionSubcategory.find(
        (x) => x.permissionSubcategoryId === 7
      );
      this.editPermission = permissionSubCategory.rolePermission[0].canEdit;
      this.deletePermission = permissionSubCategory.rolePermission[0].canDelete;
      this.addPermission = permissionSubCategory.rolePermission[0].canAdd;
      this.studentService.changePageMode(this.studentCreateMode);
      this.studentAddModel = this.studentDetailsForViewAndEdit;
      this.cloneStudentAddModel = JSON.stringify(this.studentAddModel);
      this.imageCropperService.enableUpload({
        module: this.moduleIdentifier.STUDENT,
        upload: false,
        mode: this.studentCreate.VIEW,
      });
    } else {
      this.getAllParents();
      this.studentService.changePageMode(this.studentCreateMode);
      this.studentAddModel = this.studentService.getStudentDetails();
    }
  }

  editMedicalInfo() {
    this.getAllParents();
    this.studentCreateMode = this.studentCreate.EDIT;
    this.studentService.changePageMode(this.studentCreateMode);
    this.imageCropperService.enableUpload({
      module: this.moduleIdentifier.STUDENT,
      upload: true,
      mode: this.studentCreate.VIEW,
    });
  }

  cancelEdit() {
    if (JSON.stringify(this.studentAddModel) !== this.cloneStudentAddModel) {
      this.studentAddModel = JSON.parse(this.cloneStudentAddModel);
      this.studentService.sendDetails(JSON.parse(this.cloneStudentAddModel));
    }
    this.studentCreateMode = this.studentCreate.VIEW;
    this.studentService.changePageMode(this.studentCreateMode);
    this.imageCropperService.enableUpload({
      module: this.moduleIdentifier.STUDENT,
      upload: false,
      mode: this.studentCreate.VIEW,
    });
    this.imageCropperService.cancelImage("student");
  }

  getAllParents() {
    this.parentInfoModel.studentId = this.studentAddModel.studentMaster.studentId;
    this.parentInfoService
      .ViewParentListForStudent(this.parentInfoModel)
      .subscribe((res) => {
        this.concatenateParentsName(res.parentInfoListForView);
      });
  }

  concatenateParentsName(parentDetails) {
    this.parentsFullName = parentDetails?.map((item) => {
      return item.firstname + " " + item.lastname;
    });
  }

  addAlert() {
    this.dialog.open(AddAlertComponent, {
      width: "500px",
    });
  }

  addMedicalNotes() {
    this.dialog.open(AddMedicalComponent, {
      width: "500px",
    });
  }

  addImmunization() {
    this.dialog.open(AddImmunizationComponent, {
      width: "500px",
    });
  }

  addNurseVisit() {
    this.dialog.open(AddNurseVisitComponent, {
      width: "500px",
    });
  }

  changeTab(status) {
    this.currentTab = status;
  }

  submit() {
    this.currentForm.form.markAllAsTouched();
    if (this.currentForm.form.valid) {
      if (this.studentAddModel.fieldsCategoryList !== null) {
        this.studentAddModel.selectedCategoryId = this.studentAddModel.fieldsCategoryList[
          this.categoryId
        ].categoryId;

        for (const studentCustomField of this.studentAddModel
          .fieldsCategoryList[this.categoryId].customFields) {
          if (
            studentCustomField.type === "Multiple SelectBox" &&
            this.studentService.getStudentMultiselectValue() !== undefined
          ) {
            studentCustomField.customFieldsValue[0].customFieldValue = this.studentService
              .getStudentMultiselectValue()
              .toString()
              .replaceAll(",", "|");
          }
        }
      }
      this.studentService
        .UpdateStudent(this.studentAddModel)
        .subscribe((data) => {
          if (data) {
            if (data._failure) {
              this.snackbar.open(data._message, "", {
                duration: 10000,
              });
            } else {
              this.snackbar.open(data._message, "", {
                duration: 10000,
              });
              this.studentService.setStudentCloneImage(
                data.studentMaster.studentPhoto
              );
              data.studentMaster.studentPhoto = null;
              this.cloneStudentAddModel = JSON.stringify(data);
              this.studentCreateMode = this.studentCreate.VIEW;
              this.studentService.changePageMode(this.studentCreateMode);
              this.studentDetailsForParent.emit(data);
            }
          } else {
            this.snackbar.open(
              this.defaultValuesService.translateKey(
                "medicalInformationUpdationFailed"
              ) + sessionStorage.getItem("httpError"),
              "",
              {
                duration: 10000,
              }
            );
          }
        });
    }
  }
  ngOnDestroy() {
    this.imageCropperService.enableUpload({
      module: this.moduleIdentifier.STUDENT,
      upload: false,
      mode: this.studentCreate.VIEW,
    });
  }
}
