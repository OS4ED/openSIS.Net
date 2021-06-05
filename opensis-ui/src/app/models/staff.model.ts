import { CertificateModel } from './certificate.model';
import { CommonField } from './common-field.model';
import { FieldsCategoryModel } from './fields-category.model';


export class StaffMasterModel {
    public tenantId: string;
    public schoolId: number;
    public staffId: number;
    public staffPhoto: string;
    public staffInternalId: string;
    public alternateId: string;
    public districtId: string;
    public stateId: string;
    public salutation: string;
    public firstGivenName: string;
    public middleName: string;
    public lastFamilyName: string;
    public suffix: string;
    public preferredName: string;
    public previousName: string;
    public socialSecurityNumber: string;
    public otherGovtIssuedNumber: string;
    public studentPhoto: string;
    public dob: string;
    public gender: string;
    public race: string;
    public ethnicity: string;
    public maritalStatus: string;
    public countryOfBirth: number;
    public nationality: number;
    public firstLanguage: number;
    public secondLanguage: number;
    public thirdLanguage: number;
    public physicalDisability: boolean;
    public disabilityDescription: string;
    public portalAccess: boolean;
    public loginEmailAddress: string;
    public profile: string;
    public jobTitle: string;
    public joiningDate: string;
    public endDate: string;
    public homeroomTeacher: boolean;
    public primaryGradeLevelTaught: string;
    public primarySubjectTaught: string;
    public otherGradeLevelTaught: string;
    public otherSubjectTaught: string;
    public homePhone: string;
    public mobilePhone: string;
    public officePhone: string;
    public personalEmail: string;
    public schoolEmail: string;
    public twitter: string;
    public facebook: string;
    public instagram: string;
    public youtube: string;
    public linkedin: string;
    public homeAddressLineOne: string;
    public homeAddressLineTwo: string;
    public homeAddressCity: string;
    public homeAddressState: string;
    public homeAddressCountry: string | number;
    public homeAddressZip: string;
    public mailingAddressSameToHome: boolean;
    public mailingAddressLineOne: string;
    public mailingAddressLineTwo: string;
    public mailingAddressCity: string;
    public mailingAddressState: string;
    public mailingAddressCountry: string | number;
    public status: string;
    public mailingAddressZip: string;
    public busNo: string;
    public busPickup : boolean;
    public busDropoff : boolean;
    public emergencyFirstName: string;
    public emergencyLastName: string;
    public relationshipToStaff: string;
    public emergencyHomePhone: string;
    public emergencyWorkPhone: string;
    public emergencyMobilePhone: string;
    public emergencyEmail: string;
    public lastUpdatedBy: string;
    public lastUpdated: string;
    constructor() {
        
    }
}

export class StaffAddModel extends CommonField {
    public staffMaster: StaffMasterModel;
    public fieldsCategoryList: [FieldsCategoryModel];
    public selectedCategoryId: number;
    public passwordHash: string;
    constructor() {
        super();
        this.staffMaster = new StaffMasterModel();
    }
}


export class CheckStaffInternalIdViewModel extends CommonField {
    public tenantId: string;
    public staffInternalId: string;
    public isValidInternalId: boolean;
    constructor() {
        super();
    }
}

export class StaffListModel {
    tenantId: string;
    staffId: number;
    firstGivenName: string;
    middleName: string;
    lastFamilyName: string;
    staffInternalId: string;
    profile: string;
    jobTitle: string;
    schoolEmail: string;
    mobilePhone: string;
}
export class staffMasterCloneModel extends StaffMasterModel{
    staffSchoolInfo: [StaffSchoolInfoListModel];
    checked: boolean;
}
export class GetAllStaffModel {
    getStaffListForView: [StaffListModel];
    staffMaster: [staffMasterCloneModel];
    missingAttendanceDateList=[]; // for getting first mising attendance data
    staffId: number;
    tenantId: string;
    schoolId: number;
    includeInactive: boolean;
    pageNumber: number;
    pageSize: number;
    _pageSize: number;
    totalCount: number;
    sortingModel: sorting;
    filterParams: filterParams[];
    _tenantName: string;
    _token: string;
    _failure: boolean;
    _message: string;
    _userName: string;
    public dobStartDate : Date|string;
    public dobEndDate : Date|string;
    public fullName:string;
    public profilePhoto:boolean;
    constructor() {
        this.pageNumber = 1;
        this.pageSize = 10;
        this.sortingModel = new sorting();
        this.filterParams = null;
        this._failure = false;
        this._message = "";
    }
}

class sorting {
    sortColumn: string;
    sortDirection: string;
    constructor() {
        this.sortColumn = "";
        this.sortDirection = "";
    }
}

export class filterParams {
    columnName: string;
    filterValue: string;
    filterOption: number;
    constructor() {
        this.columnName = null;
        this.filterOption = 3;
        this.filterValue = null;
    }
}

//Staff Certificate Models 
export class StaffCertificateModel extends CommonField {
    staffCertificateInfo: CertificateModel;
    constructor() {
        super();
        this.staffCertificateInfo = new CertificateModel();
    }
}
export class StaffCertificateListModel extends CommonField {
    staffCertificateInfoList: [CertificateModel];
    tenantId: string;
    schoolId: number;
    staffId: number;

    pageNumber: number;
    pageSize: number;
    filterParams: filterParams;
    constructor() {
        super();
        this.pageNumber = 1;
        this.pageSize = 10;
        this.filterParams = null;
    }
}

export class StaffSchoolInfoListModel extends CommonField {
    id: number;
    tenantId: string;
    schoolId: number;
    staffId: number;
    schoolAttachedId: number | string;
    schoolAttachedName: string;
    staffMaster: {};
    profile: string;
    startDate: Date | string;
    endDate: Date | string;
    hide: boolean; //This is not a backend key, Its only for frontend check.
    updatedBy: string;
    updatedAt: string;

    constructor() {
        super();
        this._failure = false;
    }
}
export class StaffSchoolInfoModel extends CommonField {
    schoolId: number;
    tenantId: string;
    staffId: number;
    profile: string;
    jobTitle: string;
    joiningDate: string;
    endDate: string;
    homeroomTeacher: boolean;
    primaryGradeLevelTaught: string;
    primarySubjectTaught: string;
    otherGradeLevelTaught: string;
    otherSubjectTaught: string;
    staffSchoolInfoList: [StaffSchoolInfoListModel]
    constructor() {
        super();
        this.homeroomTeacher = false;
        this.staffSchoolInfoList = [new StaffSchoolInfoListModel];
        this._failure = false;
    }
}

export class StaffMasterSearchModel {
    public tenantId: string;
    public schoolId: number;
    public staffId: number;
    public staffPhoto: string;
    public staffInternalId: string;
    public alternateId: string;
    public districtId: string;
    public stateId: string;
    public salutation: string;
    public firstGivenName: string;
    public middleName: string;
    public lastFamilyName: string;
    public suffix: string;
    public preferredName: string;
    public previousName: string;
    public socialSecurityNumber: string;
    public otherGovtIssuedNumber: string;
    public studentPhoto: string;
    public dob: string;
    public gender: string;
    public race: string;
    public ethnicity: string;
    public maritalStatus: string;
    public countryOfBirth: number;
    public nationality: number;
    public firstLanguage: number;
    public secondLanguage: number;
    public thirdLanguage: number;
    public physicalDisability: boolean;
    public disabilityDescription: string;
    public portalAccess: boolean;
    public loginEmailAddress: string;
    public profile: string;
    public jobTitle: string;
    public joiningDate: string;
    public endDate: string;
    public homeroomTeacher: boolean;
    public primaryGradeLevelTaught: string;
    public primarySubjectTaught: string;
    public otherGradeLevelTaught: string;
    public otherSubjectTaught: string;
    public homePhone: string;
    public mobilePhone: string;
    public officePhone: string;
    public personalEmail: string;
    public schoolEmail: string;
    public twitter: string;
    public facebook: string;
    public instagram: string;
    public youtube: string;
    public linkedin: string;
    public homeAddressLineOne: string;
    public homeAddressLineTwo: string;
    public homeAddressCity: string;
    public homeAddressState: string;
    public homeAddressCountry: string | number;
    public homeAddressZip: string;
    public mailingAddressSameToHome: boolean;
    public mailingAddressLineOne: string;
    public mailingAddressLineTwo: string;
    public mailingAddressCity: string;
    public mailingAddressState: string;
    public mailingAddressCountry: string | number;
    public mailingAddressZip: string;
    public busNo: string;
    public busPickup : boolean;
    public busDropoff : boolean;
    public emergencyFirstName: string;
    public emergencyLastName: string;
    public relationshipToStaff: string;
    public emergencyHomePhone: string;
    public emergencyWorkPhone: string;
    public emergencyMobilePhone: string;
    public emergencyEmail: string;
    public lastUpdatedBy: string;
    public lastUpdated: string;
    
}

export class StaffImportModel extends CommonField{
    staffAddViewModelList:StaffMasterImportModel[]
    conflictIndexNo:string;
    constructor(){
        super();
        this.staffAddViewModelList=[]
    }
}

export class StaffMasterImportModel{
    staffMaster:{}
    fieldsCategoryList:FieldsCategoryModel[];
    constructor(){
        this.staffMaster=null;
        this.fieldsCategoryList=[new FieldsCategoryModel];
    }
}

export class AfterImportStatus{
    totalStaffsSent:number;
    totalStaffsImported:number;
    totalStaffsImportedInPercentage:number;
}
