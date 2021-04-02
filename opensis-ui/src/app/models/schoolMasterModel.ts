import { schoolDetailsModel } from "../models/schoolDetailsModel";
import { CommonField } from "../models/commonField";
import { CustomFieldModel } from './customFieldModel';
import { FieldsCategoryModel } from './fieldsCategoryModel';

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
    this.tenantId = sessionStorage.getItem("tenantId");
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
    this._tenantName = sessionStorage.getItem('tenant');
    this._token = sessionStorage.getItem('token');
    this.selectedCategoryId = 0;

  }
}

export class CheckSchoolInternalIdViewModel extends CommonField {
  public tenantId: string;
  public schoolInternalId: string;
  public isValidInternalId: boolean;
  constructor() {
    super();
    this.tenantId = sessionStorage.getItem("tenantId");
    this._tenantName = sessionStorage.getItem("tenant");
    this._token = sessionStorage.getItem("token");
  }
}

