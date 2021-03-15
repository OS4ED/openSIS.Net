import { CommonField } from "../models/commonField";
import { PermissionGroup } from "./rollBasedAccessModel";



export class UserViewModel extends CommonField {
    
    public password : string;
    public email: string;
    public name: string;
    public membershipName : string;
    public membershipId:number;
    public userId?:  number;
    public tenantId: string;
    public schoolId:number;
    public permissionList: PermissionGroup[];
    constructor() {
        super();
        this.schoolId=+sessionStorage.getItem("selectedSchoolId")==0?null:+sessionStorage.getItem("selectedSchoolId");
        this.tenantId ="1E93C7BF-0FAE-42BB-9E09-A1CEDC8C0355";
        this.userId = 0;
        this.email="";
        this.password = "";
    }
}

export class CheckUserEmailAddressViewModel extends CommonField {
    public tenantId: string;
    public emailAddress: string;
    public isValidEmailAddress: boolean;
    constructor() {
        super();
        this.tenantId = sessionStorage.getItem("tenantId");
        this._tenantName = sessionStorage.getItem("tenant");
        this._token = sessionStorage.getItem("token");
    }
}

export class DataAvailablity{
    schoolLoaded:boolean;
    schoolChanged:boolean;
    dataFromUserLogin:boolean;
}
