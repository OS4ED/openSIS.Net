import { CommonField } from "./common-field.model";
import { PermissionGroup } from "./roll-based-access.model";



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
        this.tenantId ="1E93C7BF-0FAE-42BB-9E09-A1CEDC8C0355";
        this.userId = 0;
    }
}

export class CheckUserEmailAddressViewModel extends CommonField {
    public tenantId: string;
    public emailAddress: string;
    public isValidEmailAddress: boolean;
}

export class DataAvailablity{
    schoolLoaded:boolean;
    schoolChanged:boolean;
    dataFromUserLogin:boolean;
    academicYearChanged: boolean;
    academicYearLoaded: boolean;
}
