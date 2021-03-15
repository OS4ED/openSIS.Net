import { CommonField } from "../models/commonField";

export class ParentInfoModel {
    public tenantId: string;
    public schoolId: number;    
    public parentId: number;
    public parentPhoto:string;    
    public salutation: string;
    public firstname: string;
    public middlename: string;
    public lastname: string;
    public homePhone: string;
    public workPhone: string;
    public mobile: string;
    public personalEmail: string;
    public workEmail: string;
    public userProfile: string;
    public isPortalUser: boolean;   
    public loginEmail: string;
    public suffix: string;
    public busNo: string;
    public busPickup: boolean;
    public busDropoff: boolean;       
    public lastUpdated: string;
    public updatedBy: string;
    public fullAddress:string;
    public addressLineOne:string;
    public addressLineTwo:string;
    public country:string;
    public state:string;
    public city:string;
    public zip:string;
    public studentFirstName:string | any;
    public studentMiddleName:string | any;
    public studentLastName:string | any;
    public studentFullName:string|any;
    public parentAddress:[parentAddressModel];
    public students:any[];
    constructor() {
        this.parentAddress = [new parentAddressModel];
        this.tenantId = sessionStorage.getItem("tenantId");
        this.schoolId = +sessionStorage.getItem("selectedSchoolId");
    }
}

export class ParentAssociationshipModel {
    public tenantId: string;
    public schoolId: number;    
    public parentId: number;
    public studentId: number;
    public associationship:boolean;    
    public relationship: string;
    public isCustodian: boolean;
    public lastUpdated: string;
    public updatedBy: string;
    public contactType: string;
    
    constructor() {
        this.tenantId = sessionStorage.getItem("tenantId");
        this.schoolId = +sessionStorage.getItem("selectedSchoolId");
        this.updatedBy = sessionStorage.getItem("email");
    }
}
export class parentAddressModel{
    tenantId: string;
    schoolId: number;
    studentId: number;
    parentId: number;
    studentAddressSame: boolean;
    addressLineOne: string;
    addressLineTwo: string;
    country: any;
    city: string;
    state: string;
    zip: string;
    lastUpdated: string;
    updatedBy: string;
    studentMaster: {};
    constructor(){
      this.tenantId=sessionStorage.getItem("tenantId");
      this.schoolId=+sessionStorage.getItem("selectedSchoolId");      
      this.updatedBy=sessionStorage.getItem("email");
    }
  }
export class AddParentInfoModel extends CommonField {
    public parentInfo: ParentInfoModel;
    public parentAssociationship: ParentAssociationshipModel;
    public passwordHash: string;
    public getStudentForView: [];

    constructor() {
        super();
        this.parentInfo = new ParentInfoModel();
        this.parentAssociationship = new ParentAssociationshipModel();
        this._tenantName = sessionStorage.getItem("tenant");
        this._token = sessionStorage.getItem("token");
        this.getStudentForView = null;

    }
}

export class ParentInfoList extends CommonField{ 
    public parentInfoForView:[];  
    public parentInfo:[]  
    public tenantId: string;
    public schoolId: number;
    public studentId: number;
    public firstname:number;
    public lastname:number;
    public mobile:number;  
    public email: string;
    public streetAddress: string;
    public city: string;
    public state: string;
    public zip: string;
    public totalCount: string;
    public pageNumber: string;
    public _pageSize: string;   
    constructor() {  
        super();
        this.tenantId = sessionStorage.getItem("tenantId");
        this.firstname = null;
        this.lastname=null;
        this.mobile=null;
        this.email=null;
        this.streetAddress=null;
        this.city=null;
        this.state = null;
        this.zip=null;   
        this.schoolId= +sessionStorage.getItem("selectedSchoolId");       
        this._tenantName= sessionStorage.getItem("tenant");
        this._token=sessionStorage.getItem("token");
    }
}  

export class ViewParentInfoModel extends CommonField {
    parentInfoListForView: [ParentInfoModel]
    schoolId: number;
    studentId: number;
    tenantId: string;
    constructor() {
        super()
        this.schoolId = +sessionStorage.getItem("selectedSchoolId");
        this._tenantName = sessionStorage.getItem("tenant");
        this.tenantId = sessionStorage.getItem("tenantId");
        this._token = sessionStorage.getItem("token");
    }
}
export class GetAllParentModel extends CommonField {
    tenantId: string;
    schoolId: number;
    public parentInfoForView: [ParentInfoModel]
    constructor() {
        super();
        this.tenantId = sessionStorage.getItem("tenantId");
        this.schoolId = +sessionStorage.getItem("selectedSchoolId");
        this._tenantName = sessionStorage.getItem("tenant");
        this._token = sessionStorage.getItem("token");
    }
}


export class GetAllParentInfoModel extends CommonField {
    public parentInfoListForView: [];
    public tenantId: string;
    public schoolId: number;
    public studentId: number;
    constructor() {
        super();
        this.tenantId = sessionStorage.getItem("tenantId");
        this.schoolId = +sessionStorage.getItem("selectedSchoolId");       
        this._tenantName = sessionStorage.getItem("tenant");
        this._token = sessionStorage.getItem("token");
    }
}

export class AssociateStudent{
    public contactRelationship: string;
    public isCustodian: boolean;
} 

export class RemoveAssociateParent extends CommonField{
    public parentInfo: ParentInfoModel;
    public studentId: number;
    constructor() {
        super();
        this.parentInfo = new ParentInfoModel();
        this._tenantName = sessionStorage.getItem("tenant");
        this._token = sessionStorage.getItem("token");      

    }
} 

export class ParentAdvanceSearchModel{
    public firstname:string;
    public middlename:string;
    public lastname:string;
    public userProfile:string;
    public personalEmail:string;
    public workEmail:string;
    public homePhone:string;
    public mobile:string;
    public workPhone:string;
    public studentFirstName:string;
    public studentMiddleName:string;
    public studentLastName:string;
    public addressLineOne: string;
    public addressLineTwo: string;
    public country:string;
    public state: string;
    public city: string;
    public zip: string;

    constructor(){
        this.firstname='';
        this.middlename='';
        this.lastname='';
        this.userProfile='';
        this.personalEmail='';
        this.workEmail='';
        this.homePhone='';
        this.mobile='';
        this.workPhone=''
        this.studentFirstName='';
        this.studentMiddleName='';
        this.studentLastName='';
        this.addressLineOne='';
        this.addressLineTwo='';
        this.country='';
        this.state='';
        this.city='';
        this.zip='';
    }
}