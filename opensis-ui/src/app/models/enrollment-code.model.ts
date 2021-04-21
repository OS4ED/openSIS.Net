import { CommonField } from "./common-field.model";

export class EnrollmentCodeModel{
      tenantId: string;
      schoolId: number;
      enrollmentCode: number;
      academicYear: number;
      title: string;
      shortName: string;
      sortOrder: number;
      type: string;
      lastUpdated: string;
      updatedBy: string;
      constructor(){
        this.tenantId= sessionStorage.getItem('tenantId')
        this.schoolId= +sessionStorage.getItem('selectedSchoolId');
        this.enrollmentCode= 0;
        this.academicYear= +sessionStorage.getItem('academicyear');
        this.title= null;
        this.shortName= null;
        this.sortOrder= null;
        this.type= null;
        this.lastUpdated= null;
        this.updatedBy= sessionStorage.getItem('email');
      }
  }

export class EnrollmentCodeAddView extends CommonField{
  studentEnrollmentCode:EnrollmentCodeModel;
    constructor(){
        super();
        this._tenantName=sessionStorage.getItem("tenant")
        this._userName = sessionStorage.getItem("user");
        this._token=sessionStorage.getItem("token")
        this.studentEnrollmentCode=new EnrollmentCodeModel()
    }
}

export class EnrollmentCodeListView extends CommonField{
  public studentEnrollmentCodeList:[EnrollmentCodeModel];
  public schoolId:number;
  public tenantId:string;
  constructor() {
    super();
    this.studentEnrollmentCodeList=null
    this._tenantName=sessionStorage.getItem('tenant');
    this._userName = sessionStorage.getItem("user");
    this._token=sessionStorage.getItem('token'); 
    this.tenantId=sessionStorage.getItem("tenantId");
    this.schoolId=+sessionStorage.getItem("selectedSchoolId"); 
}
  //"_failure": true,
//  "_message": "string"
    

}