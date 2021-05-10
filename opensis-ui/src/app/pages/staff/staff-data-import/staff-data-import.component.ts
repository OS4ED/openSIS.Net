import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { BulkDataImport } from '../../../enums/bulk-data-import.enum';
import { ExcelService } from '../../../services/excel.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { FieldCategoryModuleEnum } from '../../../enums/field-category-module.enum';
import { FieldsCategoryListView, FieldsCategoryModel } from '../../../models/fields-category.model';
import { CustomFieldService } from '../../../services/custom-field.service';
import { CustomFieldModel } from '../../../models/custom-field.model';
import { AfterImportStatus, StaffImportModel, StaffMasterImportModel } from '../../../models/staff.model';
import { StaffService } from '../../../services/staff.service';
import { CustomFieldsValueModel } from '../../../models/custom-fields-value.model';
import { DefaultValuesService } from '../../../common/default-values.service';
import { LoaderService } from '../../../services/loader.service';
import { LanguageModel } from '../../../models/language.model';
import { CountryModel } from '../../../models/country.model';
import { GetAllSectionModel } from '../../../models/section.model';
import { LoginService } from '../../../services/login.service';
import { CommonService } from '../../../services/common.service';

@Component({
  selector: 'vex-staff-data-import',
  templateUrl: './staff-data-import.component.html',
  styleUrls: ['./staff-data-import.component.scss']
})
export class StaffDataImportComponent implements OnInit {
  bulkDataImport = BulkDataImport;
  currentTab: BulkDataImport = BulkDataImport.UPLOAD;
  fieldCategoryModuleEnum = FieldCategoryModuleEnum;
  fieldsCategoryListView: FieldsCategoryListView = new FieldsCategoryListView();
  acceptedFileTypes = "application/vnd.ms-excel,application/vnd.openxmlformats-officedocument.spreadsheetml.sheet,.csv"
  files: File[] = [];
  headers = [];
  fieldList: CustomFieldModel[] = [new CustomFieldModel()];
  jsonData = [];
  tempJsonData = []
  newHeaders = [];
  newCloneHeaders = []
  url: string;
  permissionStatus = [true, false, false, false];  //This is initial permissions for Import
  importStaffModel: StaffImportModel = new StaffImportModel();
  rejectedStaffList = [];
  toggleRejectedList:boolean=false;
  importDone:boolean=false;
  afterImportStatus: AfterImportStatus = new AfterImportStatus();
  importLoader:boolean = false;
  urlFetchLoader: boolean = false;
  failedImport:boolean = false;
  demoDownloadLink='../../../../assets/demo_docs/staff_data_for_import.xlsx'
  languages: LanguageModel = new LanguageModel();
  countryModel: CountryModel = new CountryModel();
  sectionList: GetAllSectionModel = new GetAllSectionModel();
  constructor(public translateService: TranslateService,
    private snackbar: MatSnackBar,
    private excelService: ExcelService,
    private customFieldservice: CustomFieldService,
    private staffService: StaffService,
    private defaultValueService:DefaultValuesService,
    private loginService:LoginService,
    private commonService:CommonService) {
    translateService.use('en');

   
  }

  ngOnInit(): void {
    this.getAllCustomFieldCategory();
    this.getAllLanguage();
    this.getAllCountry();
  }

  getAllLanguage() {
    this.loginService.getAllLanguage(this.languages).subscribe((res) => {
      if (res) {
        if (res._failure) {
          this.languages.tableLanguage  = [];
        }else{
          this.languages.tableLanguage=res.tableLanguage;  
        }
      }
      else {
        this.languages.tableLanguage  = [];
      }
    })
  }

  getAllCountry() {
      this.commonService.GetAllCountry(this.countryModel).subscribe(res => {
        if (res) {
          if (res._failure) {
            this.countryModel.tableCountry=[];
          } else {
            this.countryModel.tableCountry=res.tableCountry;
          }
        }
        else {
          this.countryModel.tableCountry=[];
        }
      })
  }

  onUpload(event) {
    this.resetAllPermissionsAndStorage()
    this.files.push(...event.addedFiles);
    this.onFileChange(event);
  }

  onFileChange(ev) {
    this.url = null;
    const reader = new FileReader();
    const file = ev.addedFiles[0];
    reader.onload = (event) => {
      const fileData = reader.result;
      this.jsonData = this.excelService.sheetToJson(fileData);
      this.tempJsonData = [...this.jsonData];
      this.headers = this.jsonData[0];
      this.checkForSameHeader();
    }
    reader.readAsBinaryString(file);

  }

  fetchData() {
    this.resetAllPermissionsAndStorage();
    this.urlFetchLoader=true;
    this.files = []
    let url = this.url;
    let fileData = null;
    let oReq = new XMLHttpRequest();
    oReq.open("GET", url, true);
    oReq.onerror = (res)=>{
      if(res) {
        this.snackbar.open('Please enter valid XLXS download link.', '', {
          duration: 10000
        });
        this.urlFetchLoader=false;
      }
    }
    oReq.responseType = "arraybuffer";
    oReq.onload = () => {
      let arraybuffer = oReq.response;
      / convert data to binary string /
      let data = new Uint8Array(arraybuffer);
      let arr = new Array();
      for (let i = 0; i != data.length; ++i) arr[i] = String.fromCharCode(data[i]);
      fileData = arr.join("");
      this.urlFetchLoader=false;
      this.jsonData = this.excelService.sheetToJson(fileData)
      this.tempJsonData = [...this.jsonData];
      this.headers = this.jsonData[0];
      this.checkForSameHeader();
      this.uploadFile();
    }
    oReq.send();
  }

  uploadFile() {
    this.currentTab = this.bulkDataImport.MAP;
    this.permissionStatus[this.currentTab] = true;
  }

  setNewHeadersAndMapIt() {
    this.newCloneHeaders = []
    this.newHeaders.map((item) => {
      if (!item.systemField) {
        let title = item.fieldName + '|' + item.categoryId + '|' + item.fieldId + '|' + item.type
        this.newCloneHeaders.push(title);
        return;
      }
      this.newCloneHeaders.push(item.fieldName)

    });
    let json = [];
    json = [...this.tempJsonData]
    json[0] = [...this.newCloneHeaders];
    this.jsonData = this.twoDArrayToObject(json);
    this.currentTab = this.bulkDataImport.PREVIEW;
    this.permissionStatus[this.currentTab] = true;
  }

  returnToMap() {
    this.currentTab = this.bulkDataImport.MAP;
  }

  goToImport() {
    this.importStaffModel.staffAddViewModelList = [];
    this.currentTab = this.bulkDataImport.IMPORT;
    this.permissionStatus[this.currentTab] = true;
    this.jsonData.map((item, i) => {
      this.importStaffModel.staffAddViewModelList.push(new StaffMasterImportModel())
      for (var key in item) {
        if (key.split('|')[1]) {
          let categoryIndex = this.importStaffModel.staffAddViewModelList[i].fieldsCategoryList.findIndex((cat) => {
            return cat.categoryId == +key.split('|')[1]
          });
          if (categoryIndex != -1) {
            this.pushIntoExistingFieldCategory(item, key, i, categoryIndex)
          } else {
            this.createANewFieldCategory(item, key, i);
          }
        }
      }
      this.importStaffModel.staffAddViewModelList[i].staffMaster = item;
    });

    this.importStaffModel.staffAddViewModelList.map((item) => {
      item.fieldsCategoryList = item.fieldsCategoryList?.filter(cat => cat.categoryId != 0);
      item.staffMaster = item.staffMaster
      Object.keys(item.staffMaster).filter(key => key.includes('|')).forEach(key => delete item.staffMaster[key]);
    this.replaceNamesWithId(item);  //Language,Country, Nationality 

    });
    this.importStaffList();
  }

  pushIntoExistingFieldCategory(item, key, i, categoryIndex) {
    let customField: CustomFieldModel = new CustomFieldModel();;
    customField.categoryId = +key.split('|')[1];
    customField.title = key.split('|')[0];
    customField.fieldId=+key.split('|')[2];
    customField.type= key.split('|')[3];
    this.importStaffModel.staffAddViewModelList[i].fieldsCategoryList[categoryIndex].customFields.push(customField);
    let currentCustomFieldIndex = this.importStaffModel.staffAddViewModelList[i].fieldsCategoryList[categoryIndex].customFields.length - 1;

    this.importStaffModel.staffAddViewModelList[i].fieldsCategoryList[categoryIndex]
      .customFields[currentCustomFieldIndex == -1 ? 0 : currentCustomFieldIndex]

    this.importStaffModel.staffAddViewModelList[i].fieldsCategoryList[categoryIndex]
      .customFields[currentCustomFieldIndex == -1 ? 0 : currentCustomFieldIndex]
      .customFieldsValue[0] = this.fillCustomFieldValues(item, key);
  }

  createANewFieldCategory(item, key, i) {
    let fieldCategory: FieldsCategoryModel = new FieldsCategoryModel();
    this.importStaffModel.staffAddViewModelList[i].fieldsCategoryList.push(fieldCategory)
    let currentCategoryIndex = this.importStaffModel.staffAddViewModelList[i].fieldsCategoryList.length - 1;
    fieldCategory.categoryId = +key.split('|')[1];
    this.importStaffModel.staffAddViewModelList[i].fieldsCategoryList[currentCategoryIndex] = fieldCategory;

    let customField: CustomFieldModel = new CustomFieldModel();
    customField.categoryId = +key.split('|')[1];
    customField.title = key.split('|')[0];
    customField.fieldId=+key.split('|')[2];
    customField.type= key.split('|')[3];
    this.importStaffModel.staffAddViewModelList[i].fieldsCategoryList[currentCategoryIndex].customFields.push(customField);
    let currentCustomFieldIndex = this.importStaffModel.staffAddViewModelList[i].fieldsCategoryList[currentCategoryIndex].customFields.length - 1;

    this.importStaffModel.staffAddViewModelList[i].fieldsCategoryList[currentCategoryIndex]
      .customFields[currentCustomFieldIndex == -1 ? 0 : currentCustomFieldIndex]
      .customFieldsValue[0] = this.fillCustomFieldValues(item, key);
  }


  fillCustomFieldValues(item, key) {
    let customFieldValue: CustomFieldsValueModel = new CustomFieldsValueModel();
    customFieldValue.categoryId = +key.split('|')[1];
    customFieldValue.customFieldTitle = key.split('|')[0];
    customFieldValue.customFieldValue = item[key];
    customFieldValue.fieldId = +key.split('|')[2];
    customFieldValue.module = this.fieldCategoryModuleEnum.Staff;
    customFieldValue.customFieldType = key.split('|')[3];
    customFieldValue.updatedBy=this.defaultValueService.getEmailId()
    return customFieldValue;
  }

  replaceNamesWithId(item){
    if(Object.keys(item.staffMaster).includes('firstLanguage')){
        this.findFirstLanguageNameById(item);
    }
    if(Object.keys(item.staffMaster).includes('secondLanguage')){
      this.findSecondLanguageNameById(item);
    }
    if(Object.keys(item.staffMaster).includes('thirdLanguage')){
      this.findThirdLanguageNameById(item);
    }
    if(Object.keys(item.staffMaster).includes('nationality')){
      this.replaceNationalityNameById(item);
    }
    if(Object.keys(item.staffMaster).includes('countryOfBirth')){
      this.replaceCountryNameById(item);
    }
}

findFirstLanguageNameById(item){
let firstLanguage=item.staffMaster.firstLanguage;
let firstLanguageFound = false;
for(let language of this.languages.tableLanguage){
  if(language.locale.toLowerCase()===firstLanguage.toLowerCase()){
    item.staffMaster.firstLanguage=+language.langId;
    firstLanguageFound=true;
    break;
  }
}
if(!firstLanguageFound){
  item.staffMaster.firstLanguage=null;
}
}

findSecondLanguageNameById(item){
let secondLanguage=item.staffMaster.secondLanguage;
let secondLanguageFound = false;
for(let language of this.languages.tableLanguage){
  if(language.locale.toLowerCase()===secondLanguage.toLowerCase()){
    item.staffMaster.secondLanguage=+language.langId;
    secondLanguageFound=true;
    break;
  }
}
if(!secondLanguageFound){
  item.staffMaster.secondLanguage=null;
}
}

findThirdLanguageNameById(item){
let thirdLanguage=item.staffMaster.thirdLanguage;
let thirdLanguageFound = false;
for(let language of this.languages.tableLanguage){
  if(language.locale.toLowerCase()===thirdLanguage.toLowerCase()){
    item.staffMaster.thirdLanguage=+language.langId;
    thirdLanguageFound=true;
    break;
  }
}
if(!thirdLanguageFound){
  item.staffMaster.thirdLanguage=null;
}
}

replaceNationalityNameById(item){
let nationality=item.staffMaster.nationality;
let nationalityFound = false;
for(let country of this.countryModel.tableCountry){
  if(country.name.toLowerCase()===nationality.toLowerCase()){
    item.staffMaster.nationality=+country.id;
    nationalityFound=true;
    break;
  }
}
if(!nationalityFound){
  item.staffMaster.nationality=null;
}
}

replaceCountryNameById(item){
let countryName=item.staffMaster.countryOfBirth;
let countryFound = false;
for(let country of this.countryModel.tableCountry){
  if(country.name.toLowerCase()===countryName.toLowerCase()){
    item.staffMaster.countryOfBirth=+country.id;
    countryFound=true;
    break;
  }
}
if(!countryFound){
  item.staffMaster.countryOfBirth=null;
}
}

  changeView(status) {
    if (this.permissionStatus[status]) {
      this.currentTab = status;
    }
  }

  twoDArrayToObject(TwoDArray,headerKeys?) {
    let keys = headerKeys?headerKeys:TwoDArray.shift();
    let arrayOfObjects = []
    for (let i = 0; i < TwoDArray.length; i++) {
      let tempObject = {}
      for (let j = 0; j < TwoDArray[i].length; j++) {
        tempObject[keys[j]] = TwoDArray[i][j];
      }
      arrayOfObjects.push(tempObject);
    }
    return arrayOfObjects;
  }

  getAllCustomFieldCategory() {
    this.fieldsCategoryListView.module = this.fieldCategoryModuleEnum.Staff;
    this.customFieldservice.getAllFieldsCategory(this.fieldsCategoryListView).subscribe(
      (res: FieldsCategoryListView) => {
        if (res) {
          if (res._failure) {
            if (res.fieldsCategoryList === null) {
              this.snackbar.open(res._message, '', {
                duration: 10000
              });
              this.newHeaders = []
            } else {
              this.newHeaders = []
            }
          }
          else {
            res.fieldsCategoryList.map((item) => {
              item.customFields.map((field) => {
                this.fieldList.push(field)
              })
            });
            this.fieldList.shift();

          }
        }
        else {
          this.snackbar.open('Field Category list failed. ' + sessionStorage.getItem("httpError"), '', {
            duration: 10000
          });
        }
      }
    );
  }

  checkForSameHeader(){
    for(let field of this.fieldList){
      this.headers.map((excelHeaderName,i)=>{
        if(excelHeaderName.toLowerCase()==field.title.toLowerCase()){
          this.newHeaders[i]=field;
        }
      });
    }
  }
  importStaffList() {
    this.importLoader=true;
    this.failedImport=false;
    this.rejectedStaffList=[];
    this.toggleRejectedList=false;
    this.importDone=false;
    this.staffService.addStaffList(this.importStaffModel).subscribe((res) => {
      if (res) {
        if (res._failure) {
          if (res.staffAddViewModelList === null) {
            this.snackbar.open(res._message, '', {
              duration: 10000
            });
            let rejectList = [...this.tempJsonData]
              this.rejectedStaffList=rejectList.shift();
          this.calculateImportStatus();
          } else {
            if(!res.staffAddViewModelList) return;
            res.conflictIndexNo?.split(',').map((index,i)=>{
                this.rejectedStaffList[i]=[...this.tempJsonData[+index+1]]
            })
            this.calculateImportStatus();
          }
        }
        else {
          this.calculateImportStatus();
        }
        this.importLoader=false;
        this.importDone=true;
      }
      else {
        this.snackbar.open('Staff Import failed. ' + sessionStorage.getItem("httpError"), '', {
          duration: 10000
        });
      }
      
    },
    ((error)=>{
      this.importLoader=false;
      this.failedImport = true;
    })
    )
  }

  calculateImportStatus(){
    this.afterImportStatus.totalStaffsSent = this.importStaffModel.staffAddViewModelList?.length;
    this.afterImportStatus.totalStaffsImported = this.afterImportStatus.totalStaffsSent - this.rejectedStaffList.length
    if(this.rejectedStaffList.length){
      this.afterImportStatus.totalStaffsImportedInPercentage = ((this.afterImportStatus.totalStaffsSent-this.rejectedStaffList.length)/this.afterImportStatus.totalStaffsSent)*100;
    }else{
    this.afterImportStatus.totalStaffsImportedInPercentage = 100;
    }
  }

  exportRejectionList(){
    let rejectedListInObject = [];
    rejectedListInObject=this.twoDArrayToObject(this.rejectedStaffList,this.headers);
    this.excelService.exportAsExcelFile(rejectedListInObject,'Rejected_Staffs_List_')
  }

  onRemove(event) {
    this.files.splice(this.files.indexOf(event), 1);
    this.resetAllPermissionsAndStorage()
  }

  resetAllPermissionsAndStorage() {
    this.tempJsonData=[]
    this.rejectedStaffList=[]
    this.headers = [];
    this.newHeaders = [];
    this.jsonData = [];
    this.newCloneHeaders = [];
    this.permissionStatus = [true, false, false, false];
  }


}


