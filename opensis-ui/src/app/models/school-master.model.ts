import { schoolDetailsModel } from "./school-details.model";
import { CommonField } from "./common-field.model";
import { CustomFieldModel } from './custom-field.model';
import { FieldsCategoryModel } from './fields-category.model';

export class SchoolMasterModel {

  public tenantId: string;
  public schoolId: number;
  public schoolInternalId: string;
  public schoolAltId: string;
  public schoolStateId: string
  public schoolDistrictId: string;
  public schoolLevel: string;
  public schoolClassification: string;
  public schoolName: string;
  public alternateName: string;
  public streetAddress1: string;
  public streetAddress2: string;
  public city: any;
  public county: string;
  public division: string;
  public state?: any;
  public district: string;
  public zip: string;
  public latitude?: number;
  public longitude?: number;
  public country?: any;
  public currentPeriodEnds?: number;
  public maxApiChecks?: number;
  public features: string;
  public planId?: number;
  public createdBy: string;
  public dateCreated: string;
  public modifiedBy: string;
  public dateModifed: string;
  public schoolDetail: [schoolDetailsModel];
  public fieldsCategory: FieldsCategoryModel[];
  constructor() {
    this.modifiedBy = sessionStorage.getItem("email");
    this.schoolDetail = [new schoolDetailsModel];

  }
}

export class SchoolAddViewModel extends CommonField {
  public schoolMaster: SchoolMasterModel;
  public selectedCategoryId: number;
  constructor() {
    super();
    this.schoolMaster = new SchoolMasterModel();
    this.schoolMaster.latitude = null;
    this.schoolMaster.longitude = null;
    this.selectedCategoryId = 0;

  }
}

export class CopySchoolModel extends CommonField {
  public schoolMaster: SchoolMasterModel;
  public fromSchoolId: number;
  public periods: boolean;
  public markingPeriods: boolean;
  public calendar: boolean;
  public sections: boolean;
  public rooms: boolean;
  public gradeLevels: boolean;
  public schoolFields: boolean;
  public studentFields: boolean;
  public enrollmentCodes: boolean;
  public staffFields: boolean;
  public subjets: boolean;
  public programs: boolean;
  public course: boolean;
  public attendanceCode: boolean;
  public reportCardGrades: boolean;
  public reportCardComments: boolean;
  public standardGrades: boolean;
  public honorRollSetup: boolean;
  public effortGrades: boolean;
  public profilePermission: boolean;
  public schoolLevel: boolean;
  public schoolClassification: boolean;
  public femaleToiletType: boolean;
  public femaleToiletAccessibility: boolean;
  public maleToiletType: boolean;
  public maleToiletAccessibility: boolean;
  public commonToiletType: boolean;
  public commonToiletAccessibility: boolean;
  public race: boolean;
  public ethnicity: boolean;

  constructor() {
    super();
    this.schoolMaster = new SchoolMasterModel();
    this.periods = null;
    this.markingPeriods = null;
    this.calendar = null;
    this.sections = null;
    this.rooms = null;
    this.gradeLevels = null;
    this.schoolFields = null;
    this.studentFields = null;
    this.enrollmentCodes = null;
    this.staffFields = null;
    this.subjets = null;
    this.programs = null;
    this.course = null;
    this.attendanceCode = null;
    this.reportCardGrades = null;
    this.reportCardComments = null;
    this.standardGrades = null;
    this.honorRollSetup = null;
    this.effortGrades = null;
    this.profilePermission = null;
    this.schoolLevel = null;
    this.schoolClassification = null;
    this.femaleToiletType = null;
    this.femaleToiletAccessibility = null;
    this.maleToiletType = null;
    this.maleToiletAccessibility = null;
    this.commonToiletType = null;
    this.commonToiletAccessibility = null;
    this.race = null;
    this.ethnicity = null;
  }
}

export class CheckSchoolInternalIdViewModel extends CommonField {
  public tenantId: string;
  public schoolInternalId: string;
  public isValidInternalId: boolean;
}

